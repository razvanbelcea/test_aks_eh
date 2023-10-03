using eathappy.order.business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eathappy.order.api.Controllers
{
    /// <summary>
    /// Hub controller
    /// </summary>
    [ApiController]
    [Route("hubs")]
    [Authorize]
    public class HubController : ApiBaseController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        /// <summary>
        /// Constructor of order controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="orderService"></param>
        public HubController(
            ILogger<OrderController> logger,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }
        /// <summary>
        /// Gets available hubs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAvailableHubs()
        {
            try
            {
                var hubs = await _orderService.GetAvailableHubs();

                if (hubs.Any())
                    return Ok(hubs);

                return NotFound();
            }
            catch (Exception exception)
            {
                _logger.LogError("{@get_all_orders}", exception);
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
