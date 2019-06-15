using Microsoft.EntityFrameworkCore;
using Musaca.Models;

namespace Musaca.Data
{
    public class MusacaDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProducts> OrderProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);

            modelBuilder.Entity<Product>()
                .HasKey(product => product.Id);

            modelBuilder.Entity<Order>()
                .HasKey(order => order.Id);

            modelBuilder.Entity<OrderProducts>()
                .HasKey(orderProduct => orderProduct.Id);

            modelBuilder.Entity<Order>()
                .HasMany(order => order.Products)
                .WithOne(orderProduct => orderProduct.Order)
                .HasForeignKey(orderProduct => orderProduct.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Cashier);

            base.OnModelCreating(modelBuilder);
        }
    }
}
