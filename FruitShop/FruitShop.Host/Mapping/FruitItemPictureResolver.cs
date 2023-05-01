using AutoMapper;
using FruitShop.Host.Configurations;
using FruitShop.Host.Data.Entities;
using FruitShop.Host.Models.Dtos;
using Microsoft.Extensions.Options;

namespace FruitShop.Host.Mapping
{
    public class FruitItemPictureResolver : IMemberValueResolver<FruitItemEntity, FruitItemDto, string, object>
    {
        private readonly FruitHostConfig _fruitHostConfig;

        public FruitItemPictureResolver(IOptionsSnapshot<FruitHostConfig> config)
        {
            _fruitHostConfig = config.Value;
        }

        public object Resolve(FruitItemEntity source, FruitItemDto destination, string sourceMember, object destMember, ResolutionContext context)
        {
            return $"{_fruitHostConfig.CdnHost}/{_fruitHostConfig.ImgUrl}/{sourceMember}";
        }
    }
}
