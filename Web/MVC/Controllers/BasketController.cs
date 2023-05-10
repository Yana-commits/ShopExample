using Microsoft.AspNetCore.Mvc;
using MVC.Models.Requests;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly ILogger<CatalogController> _logger;
        public BasketController(ILogger<CatalogController> logger, IBasketService basketService)
        {
            _logger = logger;
            _basketService = basketService;
        }

        public async Task<IActionResult> AddToBasket(int id, string name, decimal price)
        {
            var addItemRequest = new AddItemRequest()
            {
                Id = id,
                Name = name,
                Price = price
            };

            await _basketService.AddItemToBasket(addItemRequest);
            _logger.LogWarning($"Add to basket");
            return RedirectToAction(nameof(CatalogController.Index), "Catalog");
        }
        public async Task<IActionResult> RemoveFromBasket(int id)
        {
            var removeItemRequest = new RemoveItemRequest()
            {
                Id = id
            };

            await _basketService.RemoveFromBasket(removeItemRequest);
            _logger.LogWarning($"remove from basket");
            return RedirectToAction(nameof(CatalogController.Index), "Catalog");
        }

        public async Task<IActionResult> GetItemsFromBasket()
        {
            var result = await _basketService.GetFromBasket();
            if (result == null)
            {
                return NotFound();
            }
            _logger.LogWarning($"{result.BasketList.Count} items in basket");

            return View(result.BasketList);
        }

    }

}
