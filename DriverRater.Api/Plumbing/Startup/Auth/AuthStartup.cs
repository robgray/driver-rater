namespace DriverRater.Api.Plumbing.Startup.Auth;

using DriverRater.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class AuthStartup
{
    public static IServiceCollection AddCustomAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = configuration
            .GetSection(AuthenticationSettings.Key)
            .Get<AuthenticationSettings>();
        
        services
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.Authority = authOptions!.Authority;
                opt.Audience = authOptions!.Audience;
            });

        services.AddAuthorization(options =>
        {
            // TODO Refine Racer policy
            options.AddPolicy(AuthPolicies.Racer, policy => policy.RequireAuthenticatedUser());
            // TODO Refine Admin policy 
            options.AddPolicy(AuthPolicies.Admin, policy => policy.RequireAuthenticatedUser());
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