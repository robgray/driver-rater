namespace HelmetRanker.Plumbing.Automapper;

using System;
using System.Linq;
using HelmetRanker.Entities;
using Microsoft.Extensions.DependencyInjection;

public static class AutomapperStartup
{
    public static void AddCustomAutoMapper(this IServiceCollection services, params Type[] types) =>
        services.AddAutoMapper(types.Concat(new[] { typeof(Driver) }).ToArray());
}