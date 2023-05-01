
using FruitShop.Host.Services;
using FruitShop.Host.Services.Interfaces;
using Microsoft.Extensions.Options;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models.Dto;
using Order.Host.Models.Request;
using Order.Host.Models.Response;
using Order.Host.Repositories.Interfaces;
using Infrastructure;

namespace Order.Host.Services
{
    public class OrderService : BaseDataService<ApplicationDbContext>, IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IInternalHttpClientService _httpClient;
        private readonly IOptions<Config> _settings;

        public OrderService(
            IInternalHttpClientService httpClient,
            IDbContextWrapper<ApplicationDbContext> wrapper,
            ILogger<BaseDataService<ApplicationDbContext>> baseLogger,
            IOrderRepository repository,
            IMapper mapper,
            ILogger<OrderService> logger,
            IOptions<Config> settings)
            : base(wrapper, baseLogger)
        {
            _httpClient = httpClient;
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _settings = settings;
        }

        public async Task<int?> Add(string userId, List<BasketItem> items)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var total = 0m;
                foreach (var item in items)
                {
                    total += item.Price;
                }

                var result = await _repository.Add(
                    userId,
                    total,
                    items.Select(s => _mapper.Map<OrderItemEntity>(s)).ToList());
                if (result == null || result == 0)
                {
                    _logger.LogError("Failed_Create");
                    return 0;
                }

                _logger.LogInformation("Successful_Create");
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

                _logger.LogInformation("Successful_Deleet");
                return true;
            });
        }

        public async Task<Orders> Get(int id)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _repository.Get(id);
                if (result == null)
                {
                    _logger.LogError("Not_Found");
                    return new Orders();
                }

                return _mapper.Map<Orders>(result);
            });
        }

        public async Task<BasketResponse<Orders>> GetUserOrders(string userId, int pageIndex, int pageSize)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _repository.GetUserOrders(userId, pageIndex, pageSize);
                if (result == null)
                {
                    _logger.LogError("Not_Found");
                    return new BasketResponse<Orders>();
                }

                return new BasketResponse<Orders>
                {
                    TotalCost = result.TotalCost,
                    BasketList = result.BasketList.Select(s => _mapper.Map<Orders>(s)).ToList()
                };
            });
        }

        public async Task<bool> UpdateStatus(int id, string status)
        {
            return await ExecuteSafeAsync(async () =>
            {
                var result = await _repository.UpdateStatus(id, status);
                if (!result)
                {
                    _logger.LogError("Failed_Update");
                    return false;
                }

                _logger.LogInformation("Succssful_Update");
                return true;
            });
        }
        public async Task<object> TestToBasket()
        {
           var result = await _httpClient.SendAsync<object, object>($"{_settings.Value.BasketUrl}/testmethod",
            HttpMethod.Post,
            new { }
            );
            return result;
        }
        public async Task<BasketResponse<BasketItem>> MakeOrder(string basketId)
        {
            var result = await _httpClient.SendAsync<BasketResponse<BasketItem>, GetByIdRequest>($"{_settings.Value.BasketUrl}/getbyid",
            HttpMethod.Post,
            new GetByIdRequest()
            {
                Id = basketId,
            }
            );

            return result;
        }
    }
}
