namespace HelmetRanker.Plumbing.Validation;

using FluentValidation;
using HelmetRanker.Features.Drivers.v1.Commands;
using Microsoft.Extensions.DependencyInjection;

public static class ValidationStartup
{
    public static IServiceCollection AddCustomValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<NewDriverRank.Command>();
        
        return services;
    }
}