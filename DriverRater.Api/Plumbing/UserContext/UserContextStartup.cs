namespace DriverRater.Api.Plumbing.UserContext;

using DriverRater.Api.Services;

public static class UserContextStartup
{
    public static IServiceCollection AddCustomUserContext(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
            
        services.AddScoped(provider => new Lazy<IUserContext>(provider.GetService<IUserContext>));

        return services;
    }
}