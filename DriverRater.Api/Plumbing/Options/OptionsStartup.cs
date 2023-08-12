namespace DriverRater.Api.Plumbing.Options;

using DriverRater.Api.Options;

public static class OptionsStartup
{
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<iRacingOptions>(configuration.GetSection(iRacingOptions.Key));
        
        return services;
    }
}