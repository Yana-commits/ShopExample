using AutoMapper;
using FruitShop.Host.Services;
using FruitShop.Host.Services.Interfaces;
using Order.Host.Data;
using Order.Host.Models.Dto;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services
{
    public class OrderItemService : BaseDataService<ApplicationDbContext>, IOrderItemService
    {
        private readonly IOrderItemRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderItemService> _logger;

        public OrderItemService(
            IDbContextWrapper<ApplicationDbContext> wrapper,
            ILogger<BaseDataService<ApplicationDbContext>> baseLogger,
            IOrderItemRepository repository,
            IMapper mapper,
            ILogger<OrderItemService> logger)
            : base(wrapper, baseLogger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> Add(string name, decimal cost, int orderId)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _repository.Add(name, cost, orderId);
                if (result == null || result == default)
                {
                    _logger.LogError("Failed_Create");
                    return 0;
                }

                return result;
            });
        }

        public async Task<bool> Delete(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _repository.Delete(id);
                if (!result)
                {
                    _logger.LogError("Failed_Delete");
                    return false;
                }

                _logger.LogInformation("Successful_Delete");
                return true;
            });
        }

        public async Task<BasketItem> Get(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _repository.Get(id);
                if (result == null)
                {
                    _logger.LogError("Not_Found");
                    return new BasketItem();
                }

                return _mapper.Map<BasketItem>(result);
            });
        }
    }
}
