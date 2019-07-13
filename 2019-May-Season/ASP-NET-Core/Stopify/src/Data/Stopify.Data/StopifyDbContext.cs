using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stopify.Data.Models;

namespace Stopify.Data
{
    public class StopifyDbContext : IdentityDbContext<StopifyUser, IdentityRole, string>
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public StopifyDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
