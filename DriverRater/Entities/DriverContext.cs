namespace HelmetRanker.Entities;

using Microsoft.EntityFrameworkCore;

public class DriverContext : DbContext
{
    public DbSet<Driver> Drivers { get; set; }
}