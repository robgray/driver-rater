namespace DriverRater.Api.Plumbing.Options;

using DriverRater.Api.Settings;

public static class OptionsStartup
{
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithFluentValidation<iRacingSettings>(iRacingSettings.Key);
        services.AddOptionsWithFluentValidation<AuthenticationSettings>(AuthenticationSettings.Key);
            
        return services;
    }
}