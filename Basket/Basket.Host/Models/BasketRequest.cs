namespace Basket.Host.Models
{
    public class BasketRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
       
    }
}
