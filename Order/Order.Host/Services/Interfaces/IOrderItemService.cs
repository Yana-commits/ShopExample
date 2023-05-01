using Order.Host.Models.Dto;

namespace Order.Host.Services.Interfaces
{
    public interface IOrderItemService
    {
        public Task<int?> Add(string name, decimal cost, int orderId);
        public Task<BasketItem> Get(int id);
        public Task<bool> Delete(int id);
    }
}
