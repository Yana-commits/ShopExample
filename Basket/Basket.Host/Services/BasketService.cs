using Basket.Host.Configurations;
using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Basket.Host.Services
{
    public class BasketService : IBasketService
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<BasketService> _logger;
        private readonly IInternalHttpClientService _httpClient;
        private readonly IOptions<Config> _settings;
        public BasketService(ICacheService cacheService,
            ILogger<BasketService> logger,
             IInternalHttpClientService httpClientService,
            IOptions<Config> settings)
        {
            _cacheService = cacheService;
            _logger = logger;
            _httpClient = httpClientService;
            _settings = settings;
        }

        public async Task AddToBasket(string userId,int itemId, string name, decimal price)
        {
            var basketItem = new BasketItem()
            {
                Id = itemId,
                Name = name,
                Price = price

            };

            var result = await _cacheService.GetAsync<BasketTotal>(userId);
            if (result == null)
            {
                _logger.LogWarning("No items in the basket");
                result = new BasketTotal();
            }

            result.TotalCost += price;
            result.BasketList.Add(basketItem);

            await _cacheService.AddOrUpdateAsync<BasketTotal>(userId,result);
            _logger.LogInformation("Success");
        }

        public async Task RemoveFromBasket(string userId, int itemId)
        {
            var result = await _cacheService.GetAsync<BasketTotal>(userId);

            if (result == null)
            {
                _logger.LogError($"Value with key: {userId} — not found");
                return;
            }

            var basketItem = result.BasketList.FirstOrDefault(b => b.Id == itemId);
            if (basketItem == default)
            {
                _logger.LogError($"Value with key: {userId} ItemId: {itemId} — not found");
                return;
            }

            result.BasketList.Remove(basketItem);
            await _cacheService.AddOrUpdateAsync(userId,result);
            _logger.LogInformation("Deleted Successfull");

        }
        public async Task<BasketTotal> GetFromBasket(string userId)
        {
            var result = await _cacheService.GetAsync<BasketTotal>(userId);
            if (result == null)
            {
                _logger.LogWarning("No items in the basket");
                result = new BasketTotal();
            }
            return result;
            _logger.LogInformation($"items with id = {userId} are found");
        }
        public async Task<bool> IsInBasket(string userId, int itemId)
        {
            var result = await _cacheService.GetAsync<BasketTotal>(userId);
            if (result == null)
            {
                return false;
                _logger.LogWarning("No items in the basket");

            }
            var basketItem = result.BasketList.FirstOrDefault(b => b.Id == itemId);
            if (basketItem == null)
            {
                return false;
                _logger.LogWarning($"No such item {itemId} in basket");
            }

            return true;
            _logger.LogInformation($"item {userId} in basket");
        }
        public async Task MakeAnOrder(string userId)
        {
            var basket = await _cacheService.GetAsync<BasketTotal>(userId);
            if (basket == null)
            {
                _logger.LogError($"the Order was not found");
                return;
            }

            await _httpClient.SendAsync<object, BasketTotal>(
                $"{_settings.Value.OrderApi}/order/add",
                HttpMethod.Post,
                new UserBasket()
                {
                    UserId = userId,
                    BasketList = basket.BasketList
                });

            await RemoveCache(userId);
            _logger.LogInformation("the Сache has been cleared");
        }

        private async Task RemoveCache(string userId)
        {
            await _cacheService.Remove(userId);
        }
    }
}
