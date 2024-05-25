namespace DriverRater.Api.Extensions;

using System.Linq;
using AutoMapper;
using Serilog;

public static class MapperExtensions
{
    public static T Map<T>(this IMapper mapper, params object[] sources)
    {
        var mappedObject = sources
            .Skip(1)
            .Aggregate(mapper.Map<T>(sources[0]), (current, next) => mapper.Map(next, current));

        if (mappedObject is null)
        {
            Log.Error("Failed to AutoMap {@Sources} to output type {OutputType}", sources, typeof(T));
            throw new ApplicationException("Could not map object via AutoMapper. Check AutoMapper Profiles and try again");
        }

        return mappedObject;
    }
}