namespace Order.Host.Data.Entities
{
    public class OrderItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Cost { get; set; }
    }
}
