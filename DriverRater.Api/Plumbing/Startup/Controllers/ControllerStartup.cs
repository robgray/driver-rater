namespace DriverRater.Api.Plumbing.Startup.Controllers;

using System.Text.Json.Serialization;
using DriverRater.Api.Plumbing.Startup.Mediator;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

public static class ControllerStartup
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services, params AssemblyPart[] parts)
    {
        services
            .AddControllersWithViews(options => { options.Filters.Add<MediatorExceptionFilter>(); })
            .ConfigureApplicationPartManager(apm =>
            {
                foreach (var part in parts)
                {
                    apm.ApplicationParts.Add(part);
                }
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }
}