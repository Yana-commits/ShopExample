using Basket.Host.Configurations;
using Basket.Host.Models;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;
using FluentAssertions;
using FluentAssertions.Common;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Basket.UnitTests.Services
{
    public class BasketServiceTest
    {
        private readonly IBasketService _basketService;

        private readonly Mock<ICacheService> _cacheService;
        private readonly Mock<ILogger<BasketService>> _logger;
        private readonly Mock<IInternalHttpClientService> _internalHttp;
        private readonly Mock<IOptions<Config>> _config;

        private int itemId = 1;
        private string name = " ";
        private decimal price = 0;
        private decimal totalCoat = 0;
        private string pic = "";

        private BasketTotal userBasket = new BasketTotal()
        {
            TotalCost = 0,

            BasketList = new List<BasketItem>
            {
                new BasketItem()
                {
                    Id = 1,
                    Name = "",
                    Price = 0
                }
            }
        };

        public BasketServiceTest()
        {
            _cacheService = new Mock<ICacheService>();
            _logger = new Mock<ILogger<BasketService>>();
            _internalHttp = new Mock<IInternalHttpClientService>();
            _config = new Mock<IOptions<Config>>();

            _config.Setup(s => s.Value).Returns(new Config());

            _basketService = new BasketService(_cacheService.Object, 
                _logger.Object ,
                _internalHttp.Object,
                _config.Object);
        }

        [Fact]
        public async Task AddToBasket_Success()
        {
            // arrange
            _cacheService
                .Setup(x => x.GetAsync<BasketTotal>(It.IsAny<string>()))
                .ReturnsAsync(userBasket);

            // Act
            await _basketService.AddToBasket(It.IsAny<string>(), itemId, name, price,pic);

            // assert
            _logger.Verify(
                v => v.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Success")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_Failed()
        {
            // arrange
            BasketTotal test = null!;

            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(test);

            // act
            await _basketService.AddToBasket(It.IsAny<string>(), itemId, name, price,pic);

            // assert
            _logger.Verify(
                v => v.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("No items in the basket")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task RemoveFromBasket_Success()
        {
            // arrange
            _cacheService
                .Setup(x => x.GetAsync<BasketTotal>(It.IsAny<string>()))
                .ReturnsAsync(userBasket);

            // Act
            await _basketService.RemoveFromBasket(It.IsAny<string>(), itemId);

            // assert
            _logger.Verify(
                v => v.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Deleted Successfull")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task RemoveFromBasket_Failed_NoCache()
        {
            // arrange
            BasketTotal test = null!;

            _cacheService
                .Setup(x => x.GetAsync<BasketTotal>(It.IsAny<string>()))
                .ReturnsAsync(test);

            // Act
            await _basketService.RemoveFromBasket(It.IsAny<string>(), itemId);

            // assert
            _logger.Verify(
                v => v.Log(
                   LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!
                    .Contains($"Value with key: {It.IsAny<string>()} — not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public async Task RemoveFromBasket_Failed_NoItem()
        {
            // arrange
          
            var testBasket = new BasketTotal()
            {
                BasketList = new List<BasketItem>
                {
                    new BasketItem()
                    {
                        Name= name,
                        Price = price,
                    }
                },
                TotalCost = totalCoat,
            };
            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(testBasket);

            // act
            await _basketService.RemoveFromBasket(It.IsAny<string>(), itemId);

            // assert
            _logger.Verify(
                v => v.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains($"Value with key: {It.IsAny<string>()} ItemId: {itemId} — not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }
        [Fact]
        public async Task GetFromBasket_Success()
        {
            // arrange
            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(userBasket);

            // act
            var result = await _basketService.GetFromBasket(It.IsAny<string>());

            // assert
            result.Should().NotBeNull();
            result?.BasketList.Should().NotBeNullOrEmpty();
            result.Should().Be(userBasket);
        }

        [Fact]
        public async Task GetFromBasket_Failed()
        {
            // arrange
            BasketTotal testBasket = null!;
            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(testBasket);

            // act
            var result = await _basketService.GetFromBasket(It.IsAny<string>());

            // assert
            result.Should().NotBeNull();
            result?.BasketList.Should().BeNullOrEmpty();
            _logger.Verify(v => v.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("No items in the basket")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
        }

        [Fact]
        public async Task IsInBasket_Success()
        {
            // arrange
            bool testResult = true;

            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(userBasket);

            // act
            var result = await _basketService.IsInBasket(It.IsAny<string>(), itemId);

            // Assert
            result.Should().Be(testResult);

        }
        [Fact]
        public async Task IsInBasket_Failed()
        {
            // arrange
            bool testResult = false;
            BasketTotal testBasket = null!;

            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(testBasket);

            // act
            var result = await _basketService.IsInBasket(It.IsAny<string>(), itemId);

            // Assert
            result.Should().Be(testResult);

        }

        [Fact]
        public async Task IsInBasket_Failed_NoItem()
        {
            // arrange
            bool testResult = false;
           
            var testBasket = new BasketTotal()
            {
                BasketList = new List<BasketItem>
                {
                    new BasketItem()
                    {
                        Name= name,
                        Price = price,
                    }
                },
                TotalCost = totalCoat,
            };
            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(testBasket);

            // act
            var result = await _basketService.IsInBasket(It.IsAny<string>(), itemId);

            // Assert
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task MakeAnOrder_Successful()
        {
            // arrange
            var testUser = "Test";
            var testId = 2;
            var testName = "Test";
            var testCost = 1;
            var testBasket = new BasketTotal()
            {
                BasketList = new List<BasketItem>
                {
                    new BasketItem()
                    {
                        Id = testId,
                        Name= testName,
                        Price = testCost,
                    },
                    new BasketItem()
                    {
                        Id = testId,
                        Name= testName,
                        Price = testCost,
                    }
                },
                TotalCost = 2,
            };
            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(testBasket);
            // act
            await _basketService.MakeAnOrder(It.IsAny<string>());

            // assert
            _logger.Verify(
                v => v.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("the Сache has been cleared")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
        }

        [Fact]
        public async Task MakeAnOrder_Failed()
        {
            // arrange
            BasketTotal testBasket = null!;
            _cacheService.Setup(s => s.GetAsync<BasketTotal>(It.IsAny<string>())).ReturnsAsync(testBasket);

            // act
            await _basketService.MakeAnOrder(It.IsAny<string>());

            // assert
            _logger.Verify(v => v.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains($"the Order was not found")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
        }

    }
}
