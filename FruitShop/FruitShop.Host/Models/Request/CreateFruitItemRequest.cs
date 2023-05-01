using FruitShop.Host.Data.Entities;
using FruitShop.Host.Models.Dtos;

namespace FruitShop.Host.Models.Request
{
    public class CreateFruitItemRequest
    {
        public string Name { get; set; } = null!;
        public int FruitTypeId { get; set; }
        public int FruitSortId { get; set; }
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int ProviderId { get; set; }
        public string PictureUrl { get; set; } = null!;
    }
}
