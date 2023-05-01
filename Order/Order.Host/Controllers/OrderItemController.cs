using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Host.Models.Dto;
using Order.Host.Models.Request.Add;
using Order.Host.Models.Request;
using Order.Host.Services.Interfaces;
using System.Net;

namespace Order.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("order.orderitem.api")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _service;
        private readonly ILogger<OrderItemController> _logger;

        public OrderItemController(
            IOrderItemService service,
            ILogger<OrderItemController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(int?), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(AddOrderItemRequest request)
        {
            var result = await _service.Add(request.Name, request.Cost, request.OrderId);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(ItemIdRequest request)
        {
            var result = await _service.Get(request.Id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int?), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(ItemIdRequest request)
        {
            var result = await _service.Delete(request.Id);
            return Ok(result);
        }
    }
}
