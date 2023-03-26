namespace DriverRater.Plumbing.KeyVault;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class KeyVaultStartup
{
    public static IServiceCollection AddCustomKeyVault(this IServiceCollection services)
    {            
        services.AddOptions<KeyVaultOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(KeyVaultOptions.Key).Bind(settings);
            })
            .ValidateDataAnnotations();

        return services;
    }
}