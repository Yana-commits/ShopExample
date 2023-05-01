namespace FruitShop.Host.Models.Request
{
    public class FruitItemByTypeRequest
    {
        public string Type { get; set; } = null!;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
