namespace DriverRater.Api.Plumbing.Startup.Validation;

using DriverRater.Api.Features.Drivers.v1.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class ValidationStartup
{
    public static IServiceCollection AddCustomValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UpdateDriverRank.Command>();
        
        return services;
    }
}