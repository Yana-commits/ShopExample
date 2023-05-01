using Order.Host.Data.Entities;

namespace Order.Host.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        public Task<int?> Add(string name, decimal cost, int orderId);
        public Task<OrderItemEntity?> Get(int id);
        public Task<bool> Delete(int id);
    }
}
