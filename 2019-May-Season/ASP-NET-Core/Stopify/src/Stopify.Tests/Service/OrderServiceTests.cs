using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using Stopify.Data.Models;
using Stopify.Services;
using Stopify.Services.Mapping;
using Stopify.Services.Models;
using Stopify.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Stopify.Tests.Service
{
    public class OrderServiceTests
    {
        private IOrderService orderService;

        private List<Order> GetDummyData()
        {
            return new List<Order>
            {
                new Order
                {
                    IssuedOn = DateTime.UtcNow.AddDays(-10),
                    Quantity = 5,
                    Status = new OrderStatus
                    {
                        Name = "Active"
                    }
                },
                new Order
                {
                    IssuedOn = DateTime.UtcNow.AddDays(-15),
                    Quantity = 10,
                    Status = new OrderStatus
                    {
                        Name = "Completed"
                    }
                },
                new Order
                {
                    IssuedOn = DateTime.UtcNow.AddDays(-15),
                    Quantity = 1,
                    Status = new OrderStatus
                    {
                        Name = "Invalid Status"
                    }
                }
            };
        }

        private async Task SeedData(StopifyDbContext context)
        {
            context.AddRange(GetDummyData());
            await context.SaveChangesAsync();
        }

        public OrderServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task CreateOrder_WithCorrectData_ShouldSuccessfullyCreateOrder()
        {
            string errorMessagePrefix = "OrderService Create() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            OrderServiceModel testReceipt = new OrderServiceModel();

            bool actualResult = await this.orderService.CreateOrder(testReceipt);
            Assert.True(actualResult, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAll_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "OrderService GetAll() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            this.orderService = new OrderService(context);

            List<OrderServiceModel> actualResults = await this.orderService.GetAll().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAll_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "OrderService GetAll() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            List<OrderServiceModel> actualResults = await this.orderService.GetAll()
                .ToListAsync();
            List<OrderServiceModel> expectedResults = context.Orders
                .To<OrderServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.Quantity == actualEntry.Quantity, errorMessagePrefix + " " + "Quantity is not returned properly.");
                Assert.True(expectedEntry.IssuedOn == actualEntry.IssuedOn, errorMessagePrefix + " " + "Issued On is not returned properly.");
                Assert.True(expectedEntry.Status.Name == actualEntry.Status.Name, errorMessagePrefix + " " + "Status is not returned properly.");
            }
        }

        [Fact]
        public async Task CompleteOrder_WithExistentId_ShouldSuccessfullyCompleteOrder()
        {
            string errorMessagePrefix = "OrderService CompleteOrder() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            string testId = context.Orders.First().Id;
            await this.orderService.CompleteOrder(testId);
            Order testOrder = context.Orders.First();

            Assert.True(testOrder.Status.Name == "Completed", errorMessagePrefix);
        }

        [Fact]
        public async Task CompleteOrder_WithNonExistentId_ShouldThrowArgumentException()
        {
            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            string testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentException>(() => this.orderService.CompleteOrder(testId));
        }

        [Fact]
        public async Task CompleteOrder_WithNonActiveStatus_ShouldThrowArgumentException()
        {
            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            string testId = context.Orders.FirstOrDefault(order => order.Status.Name != "Active").Id;

            await Assert.ThrowsAsync<ArgumentException>(() => this.orderService.CompleteOrder(testId));
        }

        [Fact]
        public async Task IncreaseQuantity_WithExistentId_ShouldSuccessfullyIncreaseQuantity()
        {
            string errorMessagePrefix = "OrderService IncreaseQuantity() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            OrderServiceModel testOrder = context.Orders.First().To<OrderServiceModel>();
            await this.orderService.IncreaseQuantity(testOrder.Id);

            int expectedQuantity = testOrder.Quantity + 1;
            int actualQuantity = context.Orders.First().Quantity;

            Assert.True(expectedQuantity == actualQuantity, errorMessagePrefix);
        }

        [Fact]
        public async Task IncreaseQuantity_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            string testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.orderService.IncreaseQuantity(testId));
        }

        [Fact]
        public async Task ReduceQuantity_WithExistentId_ShouldSuccessfullyReduceQuantity()
        {
            string errorMessagePrefix = "OrderService ReduceQuantity() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            OrderServiceModel testOrder = context.Orders.First().To<OrderServiceModel>();
            await this.orderService.ReduceQuantity(testOrder.Id);

            int expectedQuantity = testOrder.Quantity - 1;
            int actualQuantity = context.Orders.First().Quantity;

            Assert.True(expectedQuantity == actualQuantity, errorMessagePrefix);
        }

        [Fact]
        public async Task ReduceQuantity_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            string testId = "Non_Existent";

            await Assert.ThrowsAsync<ArgumentNullException>(() => this.orderService.ReduceQuantity(testId));
        }

        [Fact]
        public async Task ReduceQuantity_WithExistentIdAndOneQuantity_ShouldNotReduceQuantity()
        {
            string errorMessagePrefix = "OrderService ReduceQuantity() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            this.orderService = new OrderService(context);

            OrderServiceModel testOrder = context.Orders.First(order => order.Quantity == 1).To<OrderServiceModel>();
            await this.orderService.ReduceQuantity(testOrder.Id);

            int expectedQuantity = 1;
            int actualQuantity = context.Orders.First(order => order.Id == testOrder.Id).Quantity;

            Assert.True(expectedQuantity == actualQuantity, errorMessagePrefix);
        }

        [Fact]
        public async Task SetOrdersToReceipt_WithCorrectData_ShouldSuccessfullySetOrdersToReceipt()
        {
            string errorMessagePrefix = "OrderService SetOrdersToReceipt() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();

            #region Dummy Data
            context.Orders.Add(new Order
            {
                IssuerId = "1",
                Status = new OrderStatus
                {
                    Name = "Active"
                }
            });
            context.Receipts.Add(new Receipt
            {
                RecipientId = "1"
            });
            await context.SaveChangesAsync();
            #endregion

            this.orderService = new OrderService(context);

            Receipt testReceipt = context.Receipts.First();
            await this.orderService.SetOrdersToReceipt(testReceipt);

            int expectedCountOfOrders = 1;
            int actualCountOfOrders = context.Receipts.First().Orders.Count;

            Assert.True(expectedCountOfOrders == actualCountOfOrders, errorMessagePrefix);
        }

        [Fact]
        public async Task SetOrdersToReceipt_WithDifferentOrderRecipientId_ShouldOnlyAddOrdersWithSameIssuerId()
        {
            string errorMessagePrefix = "OrderService SetOrdersToReceipt() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();

            #region Dummy Data
            context.Orders.Add(new Order
            {
                IssuerId = "1",
                Status = new OrderStatus
                {
                    Name = "Active"
                }
            });
            context.Orders.Add(new Order
            {
                IssuerId = "2",
                Status = new OrderStatus
                {
                    Name = "Active"
                }
            });
            context.Receipts.Add(new Receipt
            {
                RecipientId = "1"
            });
            await context.SaveChangesAsync();
            #endregion

            this.orderService = new OrderService(context);

            Receipt testReceipt = context.Receipts.First();
            await this.orderService.SetOrdersToReceipt(testReceipt);

            int expectedCountOfOrders = 1;
            int actualCountOfOrders = context.Receipts.First().Orders.Count;

            Assert.True(expectedCountOfOrders == actualCountOfOrders, errorMessagePrefix);
        }

        [Fact]
        public async Task SetOrdersToReceipt_WithCompletedOrderStatus_ShouldOnlyAddOrdersWithActiveStatus()
        {
            string errorMessagePrefix = "OrderService SetOrdersToReceipt() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();

            #region Dummy Data
            context.Orders.Add(new Order
            {
                IssuerId = "1",
                Status = new OrderStatus
                {
                    Name = "Active"
                }
            });
            context.Orders.Add(new Order
            {
                IssuerId = "1",
                Status = new OrderStatus
                {
                    Name = "Completed"
                }
            });
            context.Receipts.Add(new Receipt
            {
                RecipientId = "1"
            });
            await context.SaveChangesAsync();
            #endregion

            this.orderService = new OrderService(context);

            Receipt testReceipt = context.Receipts.First();
            await this.orderService.SetOrdersToReceipt(testReceipt);

            int expectedCountOfOrders = 1;
            int actualCountOfOrders = context.Receipts.First().Orders.Count;

            Assert.True(expectedCountOfOrders == actualCountOfOrders, errorMessagePrefix);
        }
    }
}
