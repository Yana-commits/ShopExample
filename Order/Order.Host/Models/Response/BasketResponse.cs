namespace Order.Host.Models.Response
{
    public class BasketResponse<T>
    {
        public decimal TotalCost { get; set; }
        public List<T> BasketList { get; set; } = new List<T>();
    }
}
