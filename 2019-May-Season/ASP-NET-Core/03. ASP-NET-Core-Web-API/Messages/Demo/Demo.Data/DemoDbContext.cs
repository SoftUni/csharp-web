using Demo.Domain;
using Microsoft.EntityFrameworkCore;

namespace Demo.Data
{
    public class DemoDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public DemoDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}