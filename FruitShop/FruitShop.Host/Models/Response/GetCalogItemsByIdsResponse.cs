using FruitShop.Host.Models.Dtos;

namespace FruitShop.Host.Models.Response
{
    public class GetCalogItemsByIdsResponse
    {
        public List<FruitItemDto> Data { get; set; } = new List<FruitItemDto>();
    }
}
