using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models.Request
{
    public class UserIdRequest
    {
        [Required]
        public string UserId { get; set; } = null!;
    }
}
