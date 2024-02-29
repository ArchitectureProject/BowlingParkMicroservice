using BowlingParkMicroService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BowlingParkMicroService.Services;

public class DataContext : DbContext
{
    public DbSet<BowlingPark> BowlingParks { get; set; }
    public DbSet<BowlingAlley> BowlingAlleys { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BowlingPark>()
            .HasKey(bowlingPark => bowlingPark.Id);
        modelBuilder.Entity<BowlingAlley>()
            .HasKey(bowlingAlley => new { bowlingAlley.BowlingParkId, bowlingAlley.AlleyNumber });
    }
}