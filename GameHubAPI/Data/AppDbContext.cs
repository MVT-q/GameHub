using GameHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameHubAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        public DbSet<Game> Games => Set<Game>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .Property(g => g.ReleaseDate)
                .HasColumnType("date");
        }
    }
}
