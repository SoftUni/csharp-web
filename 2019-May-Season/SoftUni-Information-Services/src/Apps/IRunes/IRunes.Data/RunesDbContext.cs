using IRunes.Models;

namespace IRunes.Data
{
    using Microsoft.EntityFrameworkCore;

    public class RunesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<Album> Albums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);

            modelBuilder.Entity<Track>()
                .HasKey(track => track.Id);

            modelBuilder.Entity<Album>()
                .HasKey(album => album.Id);
            modelBuilder.Entity<Album>()
                .HasMany(album => album.Tracks);

            base.OnModelCreating(modelBuilder);
        }
    }
}
