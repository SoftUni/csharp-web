using ForumSystem.Data;
using ForumSystem.Data.Models;
using ForumSystem.Data.Repositories;
using ForumSystem.Services.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ForumSystem.Services.Tests
{
    public class VotesServiceTests
    {
        [Fact]
        public async Task TwoDownVotesShouldCountOnce()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var repository = new EfRepository<Vote>(new ApplicationDbContext(options.Options));
            var service = new VotesService(repository);

            for (int i = 0; i < 100; i++)
            {
                await service.VoteAsync(1, "1", false);
            }

            for (int i = 0; i < 100; i++)
            {
                await service.VoteAsync(1, "2", false);
            }

            var votes = service.GetVotes(1);
            Assert.Equal(-2, votes);
        }
    }
}
