using FruitShop.Host.Data.Entities;
using FruitShop.Host.Models.Dtos;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace FruitShop.UnitTests.Services
{
    public class FruitItemServiceTest
    {
        private readonly IFruitItemService _fruitItemService;

        private readonly Mock<IFruitItemRepository> _fruitItemRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<FruitItemService>> _logger;
        private readonly Mock<IMapper> _mapper;

        private readonly FruitItemEntity fruitItemSuccess = new FruitItemEntity()
        {
            Name = "Name",
            FruitTypeId = 1,
            FruitSortId = 1,
            Description = "Description",
            Price = 1000,
            ProviderId = 1,
            PictureUrl = "1.png"
        };
        private readonly FruitItemDto fruitItemDtoSuccess = new FruitItemDto()
        {
            Name = "Name",
            Description = "Description",
            Price = 1000,
            PictureUrl = "1.png"
        };
        public FruitItemServiceTest()
        {
            _fruitItemRepository = new Mock<IFruitItemRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<FruitItemService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _fruitItemService = new FruitItemService(_dbContextWrapper.Object, _logger.Object, _fruitItemRepository.Object, _mapper.Object);
        }
        [Fact]
        public async Task GetFruitByIdASync_Success()
        {
            // arrange
            _fruitItemRepository.Setup(s => s.GetFruitByIdAsync(It.IsAny<int>())).ReturnsAsync(fruitItemSuccess);
            _mapper.Setup(s => s.Map<FruitItemDto>(It.Is<FruitItemEntity>(i => i.Equals(fruitItemSuccess)))).Returns(fruitItemDtoSuccess);

            // act
            var result = await _fruitItemService.GetFruitByIdASync(It.IsAny<int>());

            // assert
            result?.Should().NotBeNull();
            result?.Should().Be(fruitItemDtoSuccess);
        }

        [Fact]
        public async Task GetFruitByIdASync_Failed()
        {
            // arrange
            FruitItemEntity? testNullResult = null!;
            _fruitItemRepository.Setup(s => s.GetFruitByIdAsync(It.IsAny<int>())).ReturnsAsync(testNullResult);
            
            // act
            var result = await _fruitItemService.GetFruitByIdASync(It.IsAny<int>());

            // assert
            result.Should().BeNull();
           
        }

        [Fact]
        public async Task AddFruitItemAsync_Success()
        {
            // arrange
            var testResult = 1;

            _fruitItemRepository.Setup(s => s.Add(
           It.IsAny<string>(),
           It.IsAny<int>(),
           It.IsAny<int>(),
           It.IsAny<string>(),
           It.IsAny<decimal>(),
           It.IsAny<int>(),
           It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _fruitItemService.AddFruitItemAsync(fruitItemSuccess.Name, fruitItemSuccess.FruitTypeId, fruitItemSuccess.FruitSortId,
                fruitItemSuccess.Description, fruitItemSuccess.Price, fruitItemSuccess.ProviderId, fruitItemSuccess.PictureUrl);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            int? testResult = null;

            _fruitItemRepository.Setup(s => s.Add(
           It.IsAny<string>(),
           It.IsAny<int>(),
           It.IsAny<int>(),
           It.IsAny<string>(),
           It.IsAny<decimal>(),
           It.IsAny<int>(),
           It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _fruitItemService.AddFruitItemAsync(fruitItemSuccess.Name, fruitItemSuccess.FruitTypeId, fruitItemSuccess.FruitSortId,
                fruitItemSuccess.Description, fruitItemSuccess.Price, fruitItemSuccess.ProviderId, fruitItemSuccess.PictureUrl);

            // assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task UpdateDescriptionAsync_Success()
        {
            // arrange
            var expectedTestResult = true;
            var testDescription = "description";
            _fruitItemRepository.Setup(s => s.UpdateFruitItemAsync(It.IsAny<int>(), testDescription)).ReturnsAsync(expectedTestResult);

            // act
            var result = await _fruitItemService.UpdateDescriptionAsync(It.IsAny<int>(), testDescription);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateDescriptionAsync_Failed()
        {
            // arrange
            var expectedTestResult = false;
            var testDescription = "description";
            _fruitItemRepository.Setup(s => s.UpdateFruitItemAsync(It.IsAny<int>(), testDescription)).ReturnsAsync(expectedTestResult);

            // act
            var result = await _fruitItemService.UpdateDescriptionAsync(It.IsAny<int>(), testDescription);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteFruitItemAsync_Success()
        {
            // arrange
            var expectedTestResult = true;
            _fruitItemRepository.Setup(s => s.DeleteFruitItemAsync(It.IsAny<int>())).ReturnsAsync(expectedTestResult);

            // act
            var result = await _fruitItemService.DeleteFruitItemAsync(It.IsAny<int>());

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteFruitItemAsync_Failed()
        {
            // arrange
            var expectedTestResult = false;
            _fruitItemRepository.Setup(s => s.DeleteFruitItemAsync(It.IsAny<int>())).ReturnsAsync(expectedTestResult);

            // act
            var result = await _fruitItemService.DeleteFruitItemAsync(It.IsAny<int>());

            // assert
            result.Should().BeFalse();
        }
    }
}
