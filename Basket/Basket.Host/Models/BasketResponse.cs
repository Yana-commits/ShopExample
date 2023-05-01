namespace Basket.Host.Models
{
    public class BasketResponse<T>
    {
        public decimal TotalCost { get; set; }
        public List<T> BasketList { get; set; } = new List<T>();
    }
}
