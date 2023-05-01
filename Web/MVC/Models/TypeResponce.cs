namespace MVC.Models
{
    public class TypeResponce<T>
    {
        public long TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
