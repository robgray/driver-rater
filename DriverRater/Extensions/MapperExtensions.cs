namespace DriverRater.Extensions;

using System.Linq;
using AutoMapper;

public static class MapperExtensions
{
    public static T Map<T>(this IMapper mapper, params object[] sources)
    {
        return sources
            .Skip(1)
            .Aggregate(mapper.Map<T>(sources[0]), (current, next) => mapper.Map(next, current));
    }
}