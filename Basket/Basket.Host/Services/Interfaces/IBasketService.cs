using Basket.Host.Models;

namespace Basket.Host.Services.Interfaces
{
    public interface IBasketService
    {
        Task AddToBasket(string userId, int itemId, string name, decimal price);
        Task RemoveFromBasket(string userId, int itemId);
        Task<BasketTotal> GetFromBasket(string userId);
        Task<bool> IsInBasket(string userId, int itemId);
        Task MakeAnOrder(string userId);
    }
}
