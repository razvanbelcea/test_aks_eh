using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using DotNetCore.CAP;
using eathappy.order.business.External.DynamicPlatform.Interfaces;
using eathappy.order.business.Helper;
using eathappy.order.business.Interfaces;
using eathappy.order.data.UnitOfWork;
using eathappy.order.domain.Article;
using eathappy.order.domain.Common;
using eathappy.order.domain.Dtos.Common;
using eathappy.order.domain.Dtos.Flink;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.Dtos.Local.Result;
using eathappy.order.domain.Order;
using eathappy.order.domain.Order.Pagination;
using eathappy.order.domain.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static eathappy.order.business.Helper.ResponseCreator;

namespace eathappy.order.business.Implementations
{
    public class OrderService : IOrderService, ICapSubscribe
    {
        private readonly ILogger<OrderService> _log;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICapPublisher _capPublisher;
        private readonly IValidator<OrderDto> _orderDtoValidator;
        private readonly IDynamicsPlatformService _dynamicsPlatform;

        public OrderService(
            ILogger<OrderService> log,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICapPublisher capPublisher,
            IValidator<OrderDto> orderDtoValidator,
            IDynamicsPlatformService dynamicsPlatform
            )
        {
            _log = log;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _capPublisher = capPublisher;
            _orderDtoValidator = orderDtoValidator;
            _dynamicsPlatform = dynamicsPlatform;
        }

        public async Task<Response<OrderResultDto>> CreateOrder(OrderDto order)
        {
            Precondition.IsNotNull(order);

            var orderValidationResults = await _orderDtoValidator.ValidateAsync(order);

            if (!orderValidationResults.IsValid)
                return CreateValidationResultResponse<OrderResultDto>(null, orderValidationResults);

            var entity = _mapper.Map<Order>(order);
            _unitOfWork.OrderRepository.Add(entity);

            var result = _mapper.Map<OrderResultDto>(entity);

            return await _unitOfWork.SaveChangesAsync() ? CreateResponse(result, orderValidationResults) : null;
        }

        public async Task<bool> CreateOrderFromFlinkCsv(IFormFile file)
        {
            CsvDto csv = await GetCsvContentAsync(file);
            return await StoreOrdersToDataBase(csv);
        }

        public async Task<OrderResultDto> GetOrderById(Guid? id)
        {
            var entity = await _unitOfWork.OrderRepository.GetOrder(id);
            var orderDto = _mapper.Map<OrderResultDto>(entity);
            return orderDto;
        }

        public async Task<PagedList<Order>> GetStoreOrders(OrderParameters parameters)
        {
            var entities = await _unitOfWork.OrderRepository.GetAllStoreOrders(parameters);

            return entities;
        }

        public async Task<PagedList<Order>> GetHubOrders(OrderParameters parameters)
        {
            var entities = await _unitOfWork.OrderRepository.GetAllHubOrders(parameters);

            return entities;
        }

        public async Task<List<string>> GetAvailableHubs()
        {
            return (await _unitOfWork.OrderRepository.GetAll()).Select(x => x.HubName).Distinct().ToList();
        }

        public async Task<OrderResultDto> UpdateOrder(Guid? id, OrderUpdateDto orderDto)
        {
            var order = await _unitOfWork.OrderRepository.GetOrder(id);

            if (order == null || order.State == OrderState.Confirmed) //TODO: extract order confirmed logic and return correct message
                return null;

            using (var transaction = _unitOfWork.Database.BeginTransaction(_capPublisher, false))
            {
                if (orderDto.DeliveryDate != DateTime.MinValue) order.DeliveryDate = orderDto.DeliveryDate;
                order.State = orderDto.State;
                if (order.State == OrderState.Confirmed)
                {
                    await _capPublisher.PublishAsync("order.confirmed", _mapper.Map<OrderResultDto>(order));
                    await _capPublisher.PublishAsync("order.confirmed.csv", _mapper.Map<OrderResultDto>(order));
                }
                await _unitOfWork.SaveChangesAsync();
                transaction.Commit();
            }

            var result = _mapper.Map<OrderResultDto>(order);
            return result;
        }

        public async Task<bool> BulkUpdateStatus(Guid[] orderIds, OrderState state)
        {
            using (var transaction = _unitOfWork.Database.BeginTransaction(_capPublisher, false))
            {
                foreach (var orderId in orderIds)
                {
                    var order = await _unitOfWork.OrderRepository.GetOrder(orderId);
                    order.State = state;
                    if (order.State == OrderState.Confirmed)
                    {
                        await _capPublisher.PublishAsync("order.confirmed", _mapper.Map<OrderResultDto>(order));
                        await _capPublisher.PublishAsync("order.confirmed.csv", _mapper.Map<OrderResultDto>(order));
                    }
                }
                
                await _unitOfWork.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        //TODO: create subscriber class
        [CapSubscribe("order.created")]
        public async Task SubscribeForNewOrders(JsonElement param)
        {
            var fileName = param.GetProperty("Name").GetString();
            var content = param.GetProperty("Content").GetBytesFromBase64();

            var csv = new CsvDto { Name = fileName, Data = content };

            _ = await StoreOrdersToDataBase(csv);
        }

        private async Task<bool> StoreOrdersToDataBase(CsvDto csv)
        {
            IEnumerable<FlinkArticleDto> flinkArticles = ParseCsvForArticles(csv);
            IEnumerable<Order> orders = await ConvertToLocalOrderAsync(csv, flinkArticles);

            _unitOfWork.OrderRepository.AddRange(orders);

            return await _unitOfWork.SaveChangesAsync();
        }

        private static IEnumerable<FlinkArticleDto> ParseCsvForArticles(CsvDto csv)
        {
            using var reader = new StreamReader(new MemoryStream(csv.Data));
            var delimiter = DetectCsvDelimiter.DetectDelimiter(reader);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter
            };
            using var csvReader = new CsvReader(reader, config);
            return csvReader.GetRecords<FlinkArticleDto>().ToList();
        }

        private async Task<IEnumerable<Order>> ConvertToLocalOrderAsync(CsvDto csv, IEnumerable<FlinkArticleDto> flinkArticles)
        {
            var storeDetails = await _dynamicsPlatform.GetStoreDetails();
            var storeServedHubs = await _dynamicsPlatform.GetStoreServedHubs();

            return flinkArticles
                .GroupBy(f => new
                {
                    f.Hub,
                    f.Customer,
                    f.OrderDate,
                    f.DeliveryDate
                })
                .Select(o =>
                {
                    _ = Guid.TryParse(storeDetails.value.FirstOrDefault(c => c.cr4b7_flinkcode.cr4b7_code == o.Key.Hub)?.cr4b7_standortposid, out Guid posId);
                    _ = Guid.TryParse(storeDetails.value.FirstOrDefault(c => c.cr4b7_flinkcode.cr4b7_code == o.Key.Hub)?.cr4b7_flinkcode?.cr4b7_flinkcodeid, out Guid flinkCodeId);
                    var costCenterId = storeDetails.value.FirstOrDefault(c => c.cr4b7_flinkcode.cr4b7_code == o.Key.Hub)?.cr4b7_kostenstelle;

                    return new Order
                    {
                        HubName = o.Key.Hub,
                        CustomerId = o.Key.Customer,
                        OrderDate = o.Key.OrderDate,
                        DeliveryDate = o.Key.DeliveryDate,
                        EdiNumber = o.FirstOrDefault().EdiNumber,
                        OrderFile = csv.Name,
                        CostCenterId = costCenterId,
                        PosId = posId,
                        FlinkCodeId = flinkCodeId,
                        ServedByStore = storeServedHubs.value?.FirstOrDefault(s => s?.cr4b7_Truhe?.cr4b7_kostenstelle == costCenterId) != null,
                        Articles = o.Select(a => new Article
                        {
                            Description = a.Description,
                            Quantity = a.Quantity,
                            Sku = a.EatHappySku == 0 ? a.Sku : a.EatHappySku
                        }).ToList()
                    };
                });
        }

        private async Task<CsvDto> GetCsvContentAsync(IFormFile file)
        {
            try
            {
                using var s = new MemoryStream();
                await file.CopyToAsync(s);

                return new CsvDto
                {
                    Data = s.ToArray(),
                    Name = file.FileName,
                };
            }
            catch (Exception exception)
            {
                _log.LogError("{@get_csv_content}", exception);
                throw;
            }
        }
    }
}
