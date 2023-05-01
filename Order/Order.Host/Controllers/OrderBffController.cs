using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Host.Models.Dto;
using Order.Host.Models.Request.Update;
using Order.Host.Models.Request;
using Order.Host.Models.Response;
using Order.Host.Services.Interfaces;
using System.Net;

namespace Order.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Scope("order.orderbff.api")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderBffController : ControllerBase
    {
        private readonly ILogger<OrderBffController> _logger;
        private readonly IOrderService _service;

        public OrderBffController(
            ILogger<OrderBffController> logger,
            IOrderService orderService)
        {
            _logger = logger;
            _service = orderService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketResponse<Orders>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserOrders(PaginatedUserOrdersRequest request)
        {
            var result = await _service.GetUserOrders(request.UserId, request.PageIndex, request.PageSize);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketResponse<Orders>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateStatus(UpdateStatusRequest request)
        {
            var result = await _service.UpdateStatus(request.Id, request.Status);
            return Ok(result);
        }
    }
}
