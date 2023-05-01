namespace Order.Host.Data.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? Status { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
        public decimal TotalCost { get; set; }
    }
}
