using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models.Request
{
    public class UpdateStatusRequest : ItemIdRequest
    {
        [Required]
        public string Status { get; set; } = null!;
    }
}
