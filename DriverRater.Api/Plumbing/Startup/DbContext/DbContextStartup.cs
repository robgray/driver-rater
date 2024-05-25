namespace DriverRater.Api.Plumbing.Startup.DbContext;

using DriverRater.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

public static class DbContextStartup
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DriverRatingContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });


        if (connectionString == "SET IN appsettings.Development.json")
        {
            var logger = Log.ForContext(typeof(DbContextStartup));
            logger.Warning("Connection string was not overridden. This is ok for tests, but not for production");
        }

        return services;
    }
}