namespace DriverRater.Plumbing.Options;

using DriverRater.Options;

public static class OptionsStartup
{
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<iRacingOptions>(configuration.GetSection(iRacingOptions.Key));
        
        return services;
    }
}