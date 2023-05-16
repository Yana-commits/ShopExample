using AutoMapper;
using FruitShop.Host.Data;
using FruitShop.Host.Models.Dtos;
using FruitShop.Host.Models.Request;
using FruitShop.Host.Repositories.Interfaces;
using FruitShop.Host.Services.Interfaces;

namespace FruitShop.Host.Services
{
    public class FruitItemService : BaseDataService<ApplicationDbContext>, IFruitItemService
    {
        private readonly IFruitItemRepository _fruitItemRepository;
        private readonly IMapper _mapper;

        public FruitItemService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IFruitItemRepository fruitItemRepository,
            IMapper mapper)
            : base(dbContextWrapper, logger)
        {
            _fruitItemRepository = fruitItemRepository;
            _mapper = mapper;
        }

        public Task<int?> AddFruitItemAsync(string name, int fruitTypeId, int fruitSortId, string description,
            decimal price, int providerId, string pictureUrl)
        {
            return ExecuteSafeAsync(() => _fruitItemRepository.Add(name, fruitTypeId, fruitSortId, description,
            price, providerId, pictureUrl));
        }

        public async Task<FruitItemDto?> GetFruitByIdASync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _fruitItemRepository.GetFruitByIdAsync(id);
                var catalogItem = _mapper.Map<FruitItemDto?>(result);
                return catalogItem;
            });
        }
      
        public async Task<bool> UpdateDescriptionAsync(int id, string description)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _fruitItemRepository.UpdateFruitItemAsync(id, description);

                if (!result)
                {
                    return false;
                }

                return true;
            });
        }
        private string CheckStringToNull(string existedField ,string upadtedField)
        {
            var field = existedField;
            if (upadtedField != null )
            { 
                field = upadtedField;
            }
            return field;
        }
        private int CheckNumberToNull(int existedField,int upadtedField)
        {
            var field = existedField;
            if (upadtedField == 0)
            {
                field = upadtedField;
            }
            return field;
        }
        public async Task<bool> DeleteFruitItemAsync(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _fruitItemRepository.DeleteFruitItemAsync(id);

                if (!result)
                {
                    return false;
                }

                return true;
            });
        }

        public async Task<List<FruitItemDto>> GetFruitsByIdsAsync(List<int> ids)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _fruitItemRepository.GetFruitsByIdsAsync(ids);
                var catalogItem = result.Select(s => _mapper.Map<FruitItemDto>(s)).ToList();
                return catalogItem;
            });
        }
    }
}
