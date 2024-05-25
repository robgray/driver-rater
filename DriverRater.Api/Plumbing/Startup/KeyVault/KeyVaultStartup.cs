namespace DriverRater.Api.Plumbing.Startup.KeyVault;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class KeyVaultStartup
{
    public static IServiceCollection AddCustomKeyVault(this IServiceCollection services)
    {
        services.AddOptionsWithFluentValidation<KeyVaultOptions>(KeyVaultOptions.Key);

        return services;
    }
}