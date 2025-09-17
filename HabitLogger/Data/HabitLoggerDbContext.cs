using HabitLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HabitLogger.Data;

public class HabitLoggerDbContext : DbContext
{
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitProgress> ProgressList { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Habit>()
            .HasMany(h => h.ProgressList)
            .WithOne(p => p.Habit)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
