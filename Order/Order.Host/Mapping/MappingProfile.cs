using AutoMapper;
using Order.Host.Data.Entities;
using Order.Host.Models.Dto;

namespace Order.Host.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderEntity, Orders>();
            CreateMap<OrderItemEntity, BasketItem>();
            CreateMap<BasketItem, OrderItemEntity>();
            CreateMap<OrderEntity, Orders>();
        }
    }
}
