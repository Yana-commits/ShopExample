using MVC.Models.Requests;

namespace MVC.Services.Interfaces
{
    public interface IBasketService
    {
        Task AddItemToBasket(AddItemRequest addItem);
        Task RemoveFromBasket(RemoveItemRequest removeItemRequest);
        Task<bool> IsInBasket(IsInBasketRequest isInBasketRequest);
        Task<BasketTotal> GetFromBasket();
        Task GetFrom();
    }
}
