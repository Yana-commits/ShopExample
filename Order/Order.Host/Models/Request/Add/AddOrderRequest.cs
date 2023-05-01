using Order.Host.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models.Request.Add
{
    public class AddOrderRequest : UserIdRequest
    {
        [Required]
        public List<BasketItem> BasketList { get; set; } = new List<BasketItem>();
    }
}
