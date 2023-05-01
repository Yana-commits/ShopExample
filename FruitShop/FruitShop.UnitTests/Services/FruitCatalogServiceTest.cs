
using FruitShop.Host.Data.Entities;
using FruitShop.Host.Models.Dtos;
using FruitShop.Host.Models.Response;

namespace FruitShop.UnitTests.Services
{
    public class FruitCatalogServiceTest
    {
        private readonly IFruitCatalogService _fruitCatalogService;

        private readonly Mock<IFruitItemRepository> _fruitItemRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<FruitCatalogService>> _logger;
        private readonly Mock<IMapper> _mapper;

        public FruitCatalogServiceTest()
        {
            _fruitItemRepository = new Mock<IFruitItemRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<FruitCatalogService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _fruitCatalogService = new FruitCatalogService(_dbContextWrapper.Object, _logger.Object, _fruitItemRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetCatalogItemsAsync_Success()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 2;
            var testTotalCount = 5;

            var pagingPaginatedItemsSuccess = new PaginatedItems<FruitItemEntity>()
            {
                Data = new List<FruitItemEntity>()
            {
                new FruitItemEntity()
                {
                    Name = "TestName",
                },
            },
                TotalCount = testTotalCount,
            };

            var catalogItemSuccess = new FruitItemEntity()
            {
                Name = "TestName"
            };

            var catalogItemDtoSuccess = new FruitItemDto()
            {
                Name = "TestName"
            };

            _fruitItemRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);

            _mapper.Setup(s => s.Map<FruitItemDto>(
                It.Is<FruitItemEntity>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

            // act
            var result = await _fruitCatalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

            // assert
            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.Count.Should().Be(testTotalCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
        }
        [Fact]
        public async Task GetCatalogItemsAsync_Failed()
        {
            // arrange
            var testPageIndex = 1000;
            var testPageSize = 10000;

            _fruitItemRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<FruitItemDto>>)null!);

            // act
            var result = await _fruitCatalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCatalogByTypeAsync_Success()
        {
            // arrange
            string testType = "Fruit";
            var testTotalCount = 5;
            var testPageIndex = 0;
            var testPageSize = 2;

            var fruitItemsByTypeSuccess = new FruitItemsByType<FruitItemEntity>()
            {
                Data = new List<FruitItemEntity>()
               {
                  new FruitItemEntity()
                  {
                    Name = "TestName",
                  },
               },
                TotalCount = testTotalCount
            };

            var fruitItemSuccess = new FruitItemEntity()
            {
                Name = "TestName"
            };

            var fruitItemDtoSuccess = new FruitItemDto()
            {
                Name = "TestName"
            };

            _fruitItemRepository.Setup(s => s.GetCatalogByTypeAsync(
                It.Is<string>(i => i == testType),
                 It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))
            ).ReturnsAsync(fruitItemsByTypeSuccess);

            _mapper.Setup(s => s.Map<FruitItemDto>(
          It.Is<FruitItemEntity>(i => i.Equals(fruitItemSuccess)))).Returns(fruitItemDtoSuccess);

            //act
            var result = await _fruitCatalogService.GetCatalogByTypeAsync(testType, testPageSize ,testPageIndex);

            // assert
            result.Should().NotBeNull();
            result?.Count.Should().Be(testTotalCount);
            result?.Data.Should().NotBeNull();
        }
        [Fact]
        public async Task GetCatalogByTypeAsync_Failed()
        {
            // arrange
            string testType = "MMM";
            var testPageIndex = 0;
            var testPageSize = 2;

            _fruitItemRepository.Setup(s => s.GetCatalogByTypeAsync(
                It.Is<string>(i => i == testType),
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))
            ).Returns((Func<PaginatedItemsResponse<FruitItemDto>>)null!);

            // act
            var result = await _fruitCatalogService.GetCatalogByTypeAsync(testType, testPageSize, testPageIndex);

            // assert
            result.Should().BeNull();
        }
    }
}
