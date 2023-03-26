namespace DriverRater.Plumbing.Auth;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class AuthStartup
{
    public static IServiceCollection AddCustomAuth(this IServiceCollection services)
    {
        return services;
    }

    public static IApplicationBuilder ConfigureCustomAuth(this IApplicationBuilder app)
    {
        // app.UseAuthentication();
        // app.UseAuthorization();

        return app;
    }
}