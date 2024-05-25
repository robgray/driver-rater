namespace DriverRater.Api.Plumbing.Startup.Mapster;

using global::Mapster;

public static class MapsterStartup
{
    public static IServiceCollection AddCustomMapster(this IServiceCollection services)
    {
        services.AddMapster();
        
        return services;
    }
}