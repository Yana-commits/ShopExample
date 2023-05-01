namespace Order.Host.Models.Dto
{
    public class Orders
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime Date { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public decimal TotalCount { get; set; }
    }
}
