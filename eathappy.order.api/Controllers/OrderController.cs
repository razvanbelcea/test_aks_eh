using AutoMapper;
using eathappy.order.business.Interfaces;
using eathappy.order.domain.Dtos.Local.Parameter;
using eathappy.order.domain.Dtos.Local.Result;
using eathappy.order.domain.Order;
using eathappy.order.domain.Order.Pagination;
using eathappy.order.domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eathappy.order.api.Controllers
{
    /// <summary>
    /// Order controller
    /// </summary>
    [ApiController]
    [Route("orders")]
    [Authorize]
    public class OrderController : ApiBaseController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor of order controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="orderService"></param>
        /// <param name="mapper"></param>
        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService,
            IMapper mapper
            )
        {
            _logger = logger;
            _orderService = orderService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the order by order id
        /// </summary>
        /// <param name="orderId">The order id</param>
        /// <returns></returns>
        [HttpGet("orderId:guid")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByOrderId(Guid? orderId)
        {
            try
            {
                if (orderId.HasValue == false)
                    return BadRequest();

                var orderDto = await _orderService.GetOrderById(orderId);

                return orderDto == null ? NotFound() : Ok(orderDto);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@get_order_by_id_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Gets paginated hub orders
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.HubApprover)]
        [HttpGet("hub")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllHubOrders([FromQuery] OrderParameters parameters)
        {
            try
            {
                var orders = await _orderService.GetHubOrders(parameters);

                var metadata = new
                {
                    orders.TotalCount,
                    orders.PageSize,
                    orders.CurrentPage,
                    orders.TotalPages,
                    orders.HasNext,
                    orders.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var response = _mapper.Map<IEnumerable<OrderResultDto>>(orders);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@get_all_orders}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Gets paginated store orders
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = RoleConstants.StoreApprover)]
        [HttpGet("store")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllStoreOrders([FromQuery] OrderParameters parameters)
        {
            try
            {
                var servedByStore = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == RoleConstants.StoreApprover;

                var orders = await _orderService.GetStoreOrders(parameters);

                var metadata = new
                {
                    orders.TotalCount,
                    orders.PageSize,
                    orders.CurrentPage,
                    orders.TotalPages,
                    orders.HasNext,
                    orders.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var response = _mapper.Map<IEnumerable<OrderResultDto>>(orders);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@get_all_orders}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="orderDto">The order data</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                var result = await _orderService.CreateOrder(orderDto);

                return Result(result, orderDto);

            }
            catch (Exception exception)
            {
                _logger.LogError("{@create_order_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Create an order from Flink csv
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("createFromFlink"), DisableRequestSizeLimit]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateFromFlinkCsv(IFormFile file)
        {
            try
            {
                if (file.Length <= 0) return BadRequest();

                var result = await _orderService.CreateOrderFromFlinkCsv(file);

                return result ?
                    Ok(result) :
                    StatusCode((int)HttpStatusCode.InternalServerError);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@create_order_from_flink_csv_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDto">The order data</param>
        /// <returns></returns>
        [HttpPatch("orderId:guid")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateOrder(Guid? orderId, [FromBody] OrderUpdateDto orderDto)
        {
            try
            {
                if (orderId.HasValue == false)
                    return BadRequest();

                var result = await _orderService.UpdateOrder(orderId,orderDto);

                return (result == null) ?
                    BadRequest() :
                    Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@update_order_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        /// <summary>
        /// Bulk status update
        /// </summary>
        /// <param name="orderIds"></param>
        /// <param name="orderState"></param>
        /// <returns></returns>
        [HttpPost("bulkUpdate")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrderResultDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> BulkUpdate(Guid[] orderIds, OrderState orderState)
        {
            try
            {
                if (!orderIds.Any())
                    return BadRequest();

                var result = await _orderService.BulkUpdateStatus(orderIds, orderState);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError("{@update_order_error}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
