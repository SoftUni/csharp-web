using Microsoft.EntityFrameworkCore;
using Moq;
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
    public class ReceiptServiceTests
    {
        private IReceiptService receiptService;

        private List<Receipt> GetDummyData()
        {
            return new List<Receipt>
            {
                new Receipt
                {
                    IssuedOn = DateTime.UtcNow.AddDays(-10)
                },
                new Receipt
                {
                    IssuedOn = DateTime.UtcNow.AddDays(-15)
                }
            };
        }

        private async Task SeedData(StopifyDbContext context)
        {
            context.AddRange(GetDummyData());
            await context.SaveChangesAsync();
        }

        public ReceiptServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetAll_WithDummyData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ReceiptService GetAll() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();
            await SeedData(context);
            
            this.receiptService = new ReceiptService(context, new OrderService(context));

            List<ReceiptServiceModel> actualResults = await this.receiptService.GetAll().ToListAsync();
            List<ReceiptServiceModel> expectedResults = context.Receipts.To<ReceiptServiceModel>().ToList();

            for (int i = 0; i < expectedResults.Count; i++)
            {
                var expectedEntry = expectedResults[i];
                var actualEntry = actualResults[i];

                Assert.True(expectedEntry.IssuedOn == actualEntry.IssuedOn, errorMessagePrefix + " " + "Issued On is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAll_WithZeroData_ShouldReturnEmptyResults()
        {
            string errorMessagePrefix = "ReceiptService GetAll() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();

            this.receiptService = new ReceiptService(context, new OrderService(context));

            List<ReceiptServiceModel> actualResults = await this.receiptService.GetAll().ToListAsync();

            Assert.True(actualResults.Count == 0, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllByRecipientId_WithCorrectData_ShouldReturnCorrectResults()
        {
            string errorMessagePrefix = "ReceiptService GetAllByRecipient() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();

            #region Dummy Data

            context.Users.Add(new StopifyUser());

            await context.SaveChangesAsync();

            context.Receipts.Add(new Receipt
            {
                IssuedOn = DateTime.UtcNow.AddDays(10),
                Recipient = context.Users.First()
            });
            context.Receipts.Add(new Receipt
            {
                IssuedOn = DateTime.UtcNow.AddDays(5),
                Recipient = context.Users.First()
            });

            await context.SaveChangesAsync();

            #endregion

            this.receiptService = new ReceiptService(context, new OrderService(context));

            string testId = context.Users.First().Id;

            List<ReceiptServiceModel> actualResults = await this.receiptService.GetAllByRecipientId(testId).ToListAsync();

            Assert.True(actualResults.Count == 2, errorMessagePrefix);
        }

        [Fact]
        public async Task GetAllByRecipientId_WithDifferentRecipientIds_ShouldOnlyReturnThoseWithGivenId()
        {
            string errorMessagePrefix = "ReceiptService GetAllByRecipient() method does not work properly.";

            var context = StopifyDbContextInMemoryFactory.InitializeContext();

            #region Dummy Data

            context.Users.Add(new StopifyUser());
            context.Users.Add(new StopifyUser());

            await context.SaveChangesAsync();

            context.Receipts.Add(new Receipt
            {
                IssuedOn = DateTime.UtcNow.AddDays(10),
                Recipient = context.Users.First()
            });
            context.Receipts.Add(new Receipt
            {
                IssuedOn = DateTime.UtcNow.AddDays(5),
                Recipient = context.Users.Last()
            });

            await context.SaveChangesAsync();

            #endregion

            this.receiptService = new ReceiptService(context, new OrderService(context));

            string testId = context.Users.First().Id;

            List<ReceiptServiceModel> actualResults = await this.receiptService.GetAllByRecipientId(testId).ToListAsync();

            Assert.True(actualResults.Count == 1, errorMessagePrefix);
        }
    }
}
