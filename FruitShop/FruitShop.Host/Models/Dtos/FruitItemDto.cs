namespace FruitShop.Host.Models.Dtos
{
    public class FruitItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public FruitTypeDto FruitType { get; set; }
        public FruitSortDto FruitSort { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public ProviderDto Provider { get; set; }
        public string PictureUrl { get; set; }
    }
}
