using Microsoft.EntityFrameworkCore;
using Stopify.Data;
using System;

namespace Stopify.Tests.Common
{
    public static class StopifyDbContextInMemoryFactory
    {
        public static StopifyDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<StopifyDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            return new StopifyDbContext(options);
        }
    }
}
