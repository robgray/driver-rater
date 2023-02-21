namespace HelmetRanker.Plumbing.Mediator;

using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class MediatorStartup
{
    public static void AddCustomMediator(this IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}