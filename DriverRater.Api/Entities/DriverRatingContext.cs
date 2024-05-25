namespace DriverRater.Api.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

public class DriverRatingContext(DbContextOptions options)
    : DbContext(options)
{
    private readonly ISystemClock clock = new SystemClock();

    public DbSet<RankedDriver> Drivers { get; set; }
    
    public DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}