using AutoMapper;
using FruitShop.Host.Data;
using FruitShop.Host.Data.Entities;
using FruitShop.Host.Models.Dtos;
using FruitShop.Host.Models.Enums;
using FruitShop.Host.Models.Response;
using FruitShop.Host.Repositories.Interfaces;
using FruitShop.Host.Services.Interfaces;

namespace FruitShop.Host.Services
{
    public class FruitCatalogService : BaseDataService<ApplicationDbContext>, IFruitCatalogService
    {
        private readonly IFruitItemRepository _fruitItemRepository;
        private readonly IMapper _mapper;

        public FruitCatalogService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IFruitItemRepository fruitItemRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _fruitItemRepository = fruitItemRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedItemsResponse<FruitItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _fruitItemRepository.GetByPageAsync(pageIndex, pageSize);
                return new PaginatedItemsResponse<FruitItemDto>()
                {
                    Count = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<FruitItemDto>(s)).ToList(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            });
        }

        public async Task<FruitItemsByTypeResponse<FruitItemDto>?> GetCatalogByTypeAsync(string type, int pageSize, int pageIndex)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _fruitItemRepository.GetCatalogByTypeAsync(type,pageIndex,pageSize);
                return new FruitItemsByTypeResponse<FruitItemDto>()
                {
                    Count = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<FruitItemDto>(s)).ToList(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            });
        }

        public async Task<FruitItemsByType<FruitTypeDto>> GetTypesAsync()
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _fruitItemRepository.GetTypesAsync();

                return new FruitItemsByType<FruitTypeDto>()
                {
                    TotalCount = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<FruitTypeDto>(s)).ToList(),
                };
            });
        }
    }
}
