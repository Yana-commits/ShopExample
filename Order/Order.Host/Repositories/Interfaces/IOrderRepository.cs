using Order.Host.Data.Entities;
using Order.Host.Models.Response;

namespace Order.Host.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<int?> Add(string userId, decimal totalCost, IEnumerable<OrderItemEntity> items);
        public Task<OrderEntity?> Get(int id);
        public Task<BasketResponse<OrderEntity>?> GetUserOrders(string userId, int pageIndex, int pageSize);
        public Task<bool> UpdateStatus(int id, string status);
        public Task<bool> Delete(int id);
    }
}
