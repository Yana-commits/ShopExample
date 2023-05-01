using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using MVC.Models.Requests;
using Order.Host.Models.Request;

namespace Basket.Host.Controllers
{
    [ApiController]
    //[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    //[Scope("basket.basketCache.api")]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]

    public class BasketBffController : ControllerBase
    {
        private readonly ILogger<BasketBffController> _logger;
        private readonly IBasketService _basketService;

        public BasketBffController(
            ILogger<BasketBffController> logger,
            IBasketService basketService)
        {
            _logger = logger;
            _basketService = basketService;
        }

        [HttpPost]
       
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddItemToBasket(BasketRequest data)
        {
            var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            //basketId = "18872";
            _logger.LogWarning($"{basketId}");
            await _basketService.AddToBasket(basketId!, data.Id, data.Name, data.Price);
            return Ok();
        }

        [HttpPost]
       
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RemoveFromBasket(RemoveItemRequest removeItemRequest)
        {
            var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            //basketId = "18872";
            await _basketService.RemoveFromBasket(basketId!, removeItemRequest.Id);
            _logger.LogWarning($"you removed item with id {basketId}");
            return Ok();
        }
        [HttpPost]

        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> TestMethod()
        {
            var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            //var basketId = "18872";
            _logger.LogWarning($"you are in baasket {basketId}");

            return Ok();
        }

        [HttpPost]
       
        [ProducesResponseType(typeof(BasketResponse<BasketItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(GetByIdRequest request)
        {
            //var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            //var basketId = "18872";

            var result = await _basketService.GetFromBasket(request.Id);
            _logger.LogWarning($"you are in baasket by id {request.Id}");

            if (result == null)
            {
                _logger.LogError("NO items in basket");
                result = new BasketTotal();
            }

            return Ok(new BasketResponse<BasketItem>() { TotalCost = result.TotalCost, BasketList = result.BasketList });
        }

        [HttpPost]
        
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> IsInBasket(IsItemInBasketRequest request)
        {
            var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            //var basketId = "18872";

            var result = await _basketService.IsInBasket(basketId, request.Id);

            return Ok(result);
        }
        [HttpPost]
       
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> MakeAnOrder(UserIdRequest request)
        {
            await _basketService.MakeAnOrder(request.UserId);
            return Ok();
        }
    }
}
