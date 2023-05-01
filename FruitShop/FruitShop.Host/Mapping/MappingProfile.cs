using AutoMapper;
using FruitShop.Host.Data.Entities;
using FruitShop.Host.Models.Dtos;

namespace FruitShop.Host.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FruitItemEntity, FruitItemDto>()
            .ForMember("PictureUrl", opt
                => opt.MapFrom<FruitItemPictureResolver, string>(c => c.PictureUrl));
            CreateMap<FruitSortEntity, FruitSortDto>();
            CreateMap<FruitTypeEntity, FruitTypeDto>();
            CreateMap<ProviderEntity, ProviderDto>();
        }
    }
}
