using MVC.Models;
using MVC.Models.Requests;
using MVC.Services.Interfaces;
using System.Net.Http;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<CatalogService> _logger;

        public BasketService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }
        public async Task AddItemToBasket(AddItemRequest addItem)
        {
            await _httpClient.SendAsync<object, AddItemRequest>($"{_settings.Value.BasketUrl}/additemtobasket",
                HttpMethod.Post,
                addItem);
        }
        public async Task RemoveFromBasket(RemoveItemRequest removeItemRequest)
        {
            await _httpClient.SendAsync<object, RemoveItemRequest>($"{_settings.Value.BasketUrl}/removefrombasket",
                   HttpMethod.Post,
                   removeItemRequest);
        }
        public async Task<bool> IsInBasket(IsInBasketRequest isInBasketRequest)
        {
            var result = await _httpClient.SendAsync<bool, IsInBasketRequest>($"{_settings.Value.BasketUrl}/isinbasket",
                HttpMethod.Post,
               isInBasketRequest);

            return result;
        }
        public async Task<BasketTotal> GetFromBasket()
        {
            var result = await _httpClient.SendAsync<BasketTotal, object>($"{_settings.Value.BasketUrl}/getbyid",
                HttpMethod.Post,
              new { });

            return result;
        }
       
        public async Task MakeAnOrder()
        {
            await _httpClient.SendAsync<object, object>($"{_settings.Value.BasketUrl}/makeanorder",
               HttpMethod.Post,
             new { });

            _logger.LogWarning("nu ok");
        }
    }
}
