namespace MVC.Models.Requests
{
    public class GetItemsByIdsRequest
    {
        public List<int> IdsList { get; set; } = new List<int>();
    }
}
