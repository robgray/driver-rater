namespace DriverRater.Api.Plumbing.Auth;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class AuthStartup
{
    public static IServiceCollection AddCustomAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication()
            .AddJwtBearer();

        services.AddAuthorization(options =>
        {
            // options.AddPolicy("read", policy => policy.Requirements.Add(new AssertionRequirement(f => f.)));
        });

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        
        return services;
    }

    public static IApplicationBuilder UseCustomAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}