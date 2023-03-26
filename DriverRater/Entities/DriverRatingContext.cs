namespace DriverRater.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

public class DriverRatingContext : DbContext
{
    private readonly ISystemClock clock;
    
    public DriverRatingContext(DbContextOptions options) : base(options)
    {
        clock = new SystemClock();
    }

    public DbSet<RankedDriver> Drivers { get; set; }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}