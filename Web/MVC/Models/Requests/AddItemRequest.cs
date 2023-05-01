namespace MVC.Models.Requests
{
    public class AddItemRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
