namespace DriverRater.Api.Plumbing.Automapper;

using System;
using System.Linq;
using DriverRater.Api.Entities;
using Microsoft.Extensions.DependencyInjection;

public static class AutomapperStartup
{
    public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services, params Type[] types)
    {
        services.AddAutoMapper(types.Concat(new[] { typeof(RankedDriver) }).ToArray());

        return services;
    }
}