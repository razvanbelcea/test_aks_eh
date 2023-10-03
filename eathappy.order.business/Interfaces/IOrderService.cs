using eathappy.order.domain.Common;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.Dtos.Local.Result;
using eathappy.order.domain.Order;
using eathappy.order.domain.Order.Pagination;
using eathappy.order.domain.Pagination;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eathappy.order.business.Interfaces
{
    public interface IOrderService
    {
        Task<PagedList<Order>> GetStoreOrders(OrderParameters parameters);
        Task<PagedList<Order>> GetHubOrders(OrderParameters parameters);
        Task<OrderResultDto> GetOrderById(Guid? id);
        Task<Response<OrderResultDto>> CreateOrder(OrderDto order);
        Task<bool> CreateOrderFromFlinkCsv(IFormFile file);
        Task<OrderResultDto> UpdateOrder(Guid? id, OrderUpdateDto order);
        Task<List<string>> GetAvailableHubs();
        Task<bool> BulkUpdateStatus(Guid[] orderIds, OrderState state);
    }
}
