namespace HelmetRanker.Plumbing.Controllers;

using System.Text.Json.Serialization;
using FluentValidation;
using HelmetRanker.Plumbing.Mediator;
using HelmetRanker.Plumbing.Validation;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

public static class ControllerStartup
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services, params AssemblyPart[] parts)
    {
        services
            .AddCustomValidation()
            .AddControllersWithViews(options => { options.Filters.Add<MediatorExceptionFilter>(); })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .ConfigureApplicationPartManager(apm =>
            {
                foreach (var part in parts)
                {
                    apm.ApplicationParts.Add(part);
                }
            });
        
        return services;
    }
}