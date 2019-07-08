using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyFirstMvcApp.Data;
using MyFirstMvcApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFirstMvcApp.Tests
{
    public class UsersServiceTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var users = new List<IdentityUser>
            {
                new IdentityUser { UserName = "Pesho1", Email = "abv@abv.bg" },
                new IdentityUser { UserName = "Pesho2", Email = "zzzzzz@abv.bg" },
                new IdentityUser { UserName = "Pesho3", Email = "pppppp@abv.bg" },
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                                    .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            foreach (var user in users)
            {
                await dbContext.Users.AddAsync(user);
            }

            await dbContext.SaveChangesAsync();
            return dbContext;
        }

        [Fact]
        public async Task LatestUsernameShouldReturnCorrectValuesWhenSortedByEmail()
        {
            var dbContext = await this.GetDbContext();
            var service = new UsersService(dbContext);

            var actual = service.LatestUsername("email");
            Assert.Equal("Pesho2", actual);
        }
    }
}
