using FruitShop.Host.Data;
using FruitShop.Host.Models.Dtos;
using FruitShop.Host.Models.Enums;
using FruitShop.Host.Models.Response;

namespace FruitShop.Host.Services.Interfaces
{
    public interface IFruitCatalogService
    {
        Task<PaginatedItemsResponse<FruitItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
        Task<FruitItemsByTypeResponse<FruitItemDto>?> GetCatalogByTypeAsync(string type, int pageSize, int pageIndex);
        Task<FruitItemsByType<FruitTypeDto>> GetTypesAsync();
    }
}
