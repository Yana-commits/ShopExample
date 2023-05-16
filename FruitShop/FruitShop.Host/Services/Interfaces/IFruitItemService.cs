using FruitShop.Host.Models.Dtos;

namespace FruitShop.Host.Services.Interfaces
{
    public interface IFruitItemService
    {
        public Task<int?> AddFruitItemAsync(string name, int fruitTypeId, int fruitSortId, string description,
            decimal price, int providerId, string pictureUrl);
        Task<FruitItemDto?> GetFruitByIdASync(int id);
        Task<bool> UpdateDescriptionAsync(int id, string description);
        Task<bool> DeleteFruitItemAsync(int id);
        Task<List<FruitItemDto>> GetFruitsByIdsAsync(List<int> ids);
    }
}
