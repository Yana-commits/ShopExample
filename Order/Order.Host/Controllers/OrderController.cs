
using Infrastructure;
using Microsoft.Extensions.Options;
using Order.Host.Models.Dto;
using Order.Host.Models.Request;
using Order.Host.Models.Request.Add;
using Order.Host.Models.Request.Update;
using Order.Host.Models.Response;

namespace Order.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("order.order.api")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrderController> _logger;
        private readonly IOptions<Config> _settings;

        public OrderController(
            IOrderService orderService,
            ILogger<OrderController> logger,
            IOptions<Config> settings)
        { 
            _service = orderService;
            _logger = logger;
            _settings = settings;
        }

        [HttpPost]
        
        [ProducesResponseType(typeof(int?), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(AddOrderRequest request)
        {
            var result = await _service.Add(request.UserId, request.BasketList);
            _logger.LogWarning("In OrderController");
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(ItemIdRequest request)
        {
            await _service.Delete(request.Id);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(Orders), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(ItemIdRequest request)
        {
            var result = await _service.Get(request.Id);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int?), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserOrders(PaginatedUserOrdersRequest request)
        {
            var result = await _service.GetUserOrders(request.UserId, request.PageIndex, request.PageSize);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateStatus(UpdateStatusRequest request)
        {
            await _service.UpdateStatus(request.Id, request.Status);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(int?), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TestToBasket()
        {
            await _service.TestToBasket();
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketResponse<BasketItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> MakeOrder()
        {
            var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            //var basketId = "18872";
            var result = await _service.MakeOrder(basketId);

            return Ok(result);
        }
    }
}
