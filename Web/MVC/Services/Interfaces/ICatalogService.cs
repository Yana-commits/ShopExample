using MVC.Models.Requests;
using MVC.ViewModels;

namespace MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type);
    Task<IEnumerable<SelectListItem>> GetTypes();
    Task<CatalogItem> GetItemById(int id);
    Task AddItemToBasket(AddItemRequest addItem);
    Task RemoveFromBasket(RemoveItemRequest removeItemRequest);
    Task<bool> IsInBasket(IsInBasketRequest isInBasketRequest);
}
