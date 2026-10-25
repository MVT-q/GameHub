using GameHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameHubAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        public DbSet<Game> Games => Set<Game>();

        public DbSet<GameLibraryEntry> GameLibraryEntries => Set<GameLibraryEntry>();

        public DbSet<GameLibraryNote> GameLibraryNotes => Set<GameLibraryNote>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .Property(g => g.ReleaseDate)
                .HasColumnType("date");

            modelBuilder.Entity<Game>()
                .HasMany(g => g.GameLibraryEntries)
                .WithOne(gle => gle.Game)
                .HasForeignKey(gle => gle.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.GameLibraryEntries)
                .WithOne(gle => gle.User)
                .HasForeignKey(gle => gle.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLibraryEntry>()
                .HasMany(gle => gle.Notes)
                .WithOne(n => n.GameLibraryEntry)
                .HasForeignKey(n => n.GameLibraryEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLibraryEntry>()
                .HasIndex(gle => new { gle.UserId, gle.GameId })
                .IsUnique();
        }
    }
}
