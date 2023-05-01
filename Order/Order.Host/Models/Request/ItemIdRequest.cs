using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models.Request
{
    public class ItemIdRequest
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
    }
}
