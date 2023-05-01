using FruitShop.Host.Data.Entities;

namespace FruitShop.Host.Models.Request
{
    public class UpdateFruitItemRequest
    {
        public int Id { get; set; }
        public string Description { get; set; } = null;
    }
}
