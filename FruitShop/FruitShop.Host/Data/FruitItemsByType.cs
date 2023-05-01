namespace FruitShop.Host.Data
{
    public class FruitItemsByType<T>
    {
        public long TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
