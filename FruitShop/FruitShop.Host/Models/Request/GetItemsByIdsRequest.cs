namespace FruitShop.Host.Models.Request
{
    public class GetItemsByIdsRequest
    {
        public List<int> IdsList { get; set; } = new List<int>();
    }
}
