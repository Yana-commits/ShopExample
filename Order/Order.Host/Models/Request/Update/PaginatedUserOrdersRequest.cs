using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models.Request.Update
{
    public class PaginatedUserOrdersRequest : UserIdRequest
    {
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; }
        [Range(0, int.MaxValue)]
        public int PageSize { get; set; }
    }
}
