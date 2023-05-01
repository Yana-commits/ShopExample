namespace FruitShop.Host.Models.Response
{
    public class AddFruitItemResponse<T>
    {
        public T Id { get; set; } = default(T)!;
    }
}
