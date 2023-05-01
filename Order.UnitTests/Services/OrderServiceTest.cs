using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Order.Host.Configurations;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models.Dto;
using Order.Host.Models.Request;
using Order.Host.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Order.UnitTests.Services
{
    public class OrderServiceTest
    {
        private readonly IOrderService _service;

        private readonly Mock<IOrderRepository> _repository;
        private readonly Mock<ILogger<OrderService>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _wrapper;
        private readonly Mock<IInternalHttpClientService> _internalHttp;
        private readonly Mock<IOptions<Config>> _settings;

        public OrderServiceTest()
        {
            _internalHttp = new Mock<IInternalHttpClientService>();
            _repository = new Mock<IOrderRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<OrderService>>();
            _wrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _wrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _settings = new Mock<IOptions<Config>>();
            _settings.Setup(s => s.Value).Returns(new Config());

            _service = new OrderService(
                 _internalHttp.Object,
                 _wrapper.Object,
                 _logger.Object,
                 _repository.Object,
                 _mapper.Object,
                 _logger.Object,
                 _settings.Object);
        }

        [Fact]
        public async Task Add_Succesful()
        {
            // arrange
            var testId = 5;
            var testUserId = "Test";
            var testOrderItem = new BasketItem()
            {
                Id = testId,
                Name = "Test",
                Price = 1,
            };
            var testOrderItemEntity = new OrderItemEntity()
            {
                Id = testId,
                Name = "Test",
                Cost = 1,
            };
            var testList = new List<BasketItem>
            {
                testOrderItem
            };
            var testListEntity = new List<OrderItemEntity>
            {
                testOrderItemEntity
            };
            _repository.Setup(s => s.Add(testUserId, It.IsAny<decimal>(), testListEntity)).ReturnsAsync(testId);
            _mapper.Setup(s => s.Map<OrderItemEntity>(It.Is<BasketItem>(i => i.Equals(testOrderItem)))).Returns(testOrderItemEntity);

            // act
            var result = await _service.Add(testUserId, testList);

            // assert
            result.Should().NotBeNull();
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Add_Failed()
        {
            // arrange
            int? testId = null!;
            var testUserId = "Test";
            var testOrderItem = new BasketItem()
            {
                Id = 5,
                Name = "Test",
                Price = 1,
            };
            var testOrderItemEntity = new OrderItemEntity()
            {
                Id = 5,
                Name = "Test",
                Cost = 1,
            };
            var testList = new List<BasketItem>
            {
                testOrderItem
            };
            var testListEntity = new List<OrderItemEntity>
            {
                testOrderItemEntity
            };
            _repository.Setup(s => s.Add(testUserId, It.IsAny<decimal>(), testListEntity)).ReturnsAsync(testId);
            _mapper.Setup(s => s.Map<OrderItemEntity>(It.Is<BasketItem>(i => i.Equals(testOrderItem)))).Returns(testOrderItemEntity);

            // act
            var result = await _service.Add(testUserId, testList);

            // assert
            result.Should().NotBeNull();
            result.Should().Be(0);
        }

        [Fact]
        public async Task Delete_Succesful()
        {
            // arrange
            var testResult = true;
            _repository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _service.Delete(It.IsAny<int>());

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Delete_Failed()
        {
            // arrange
            var testResult = false;
            _repository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

            // act
            var result = await _service.Delete(It.IsAny<int>());

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Get_Succesful()
        {
            // arrange
            var testItem = new Orders
            {
                Id = 5,
                Status = "created",
                Date = new DateTime(2000, 1, 1),
                UserId = "Test"
            };
            var testItemEntity = new OrderEntity
            {
                Id = 5,
                Status = "created",
                Date = new DateTime(2000, 1, 1),
                UserId = "Test"
            };
            _repository.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(testItemEntity);
            _mapper.Setup(s => s.Map<Orders>(It.Is<OrderEntity>(i => i.Equals(testItemEntity)))).Returns(testItem);

            // act
            var result = await _service.Get(It.IsAny<int>());

            // assert
            result.Should().NotBeNull();
            result?.Status.Should().Be("created");
            result?.UserId.Should().Be("Test");
        }

        [Fact]
        public async Task Get_Failed()
        {
            // arrange
            OrderEntity testItemEntity = null!;
            _repository.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(testItemEntity);

            // act
            var result = await _service.Get(It.IsAny<int>());

            // assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetUserOrders_Succesful()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 5;
            var testUserId = "Test";
            var testOrderEntity = new BasketResponse<OrderEntity>
            {
                TotalCost = 1,
                BasketList = new List<OrderEntity>
                {
                    new OrderEntity()
                }
            };
            var testOrder = new BasketResponse<Orders>
            {
                TotalCost = 1,
                BasketList = new List<Orders>
                {
                    new Orders()
                }
            };
            _repository.Setup(s => s.GetUserOrders(testUserId, testPageIndex, testPageSize)).ReturnsAsync(testOrderEntity);
            _mapper.Setup(s => s.Map<Orders>(It.Is<OrderEntity>(i => i.Equals(new OrderEntity())))).Returns(new Orders());

            // act
            var result = await _service.GetUserOrders(testUserId, testPageIndex, testPageSize);

            // assert
            result.Should().NotBeNull();
            result?.TotalCost.Should().Be(1);
            result?.BasketList.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetUserOrders_Failed()
        {
            // arrange
            var testPageIndex = 0;
            var testPageSize = 5;
            var testUserId = "Test";
            BasketResponse<OrderEntity> testOrderEntity = null!;
            _repository.Setup(s => s.GetUserOrders(testUserId, testPageIndex, testPageSize)).ReturnsAsync(testOrderEntity);

            // act
            var result = await _service.GetUserOrders(testUserId, testPageIndex, testPageSize);

            // assert
            result.Should().NotBeNull();
            result?.TotalCost.Should().Be(0);
            result?.BasketList.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task UpdateStatus_Succesful()
        {
            // arrange
            var testResult = true;
            _repository.Setup(s => s.UpdateStatus(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _service.UpdateStatus(It.IsAny<int>(), It.IsAny<string>());

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateStatus_Failed()
        {
            // arrange
            var testResult = false;
            _repository.Setup(s => s.UpdateStatus(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(testResult);

            // act
            var result = await _service.UpdateStatus(It.IsAny<int>(), It.IsAny<string>());

            // assert
            result.Should().BeFalse();
        }
    }
}
