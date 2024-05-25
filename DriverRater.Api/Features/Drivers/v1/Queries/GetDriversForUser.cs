namespace DriverRater.Api.Features.Drivers.v1.Queries;

using DriverRater.Api.Entities;
using DriverRater.Api.Plumbing.Startup.Mediator;
using DriverRater.Shared;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

[UsedImplicitly]
public class GetDriversForUser
{
    [UsedImplicitly]
    public record Query : IQuery<IEnumerable<RankedDriver>>
    {
        public IUserContext? UserContext { get; init; }
    }

    [UsedImplicitly]
    public class QueryHandler(DriverRatingContext ratingContext) : IQueryHandler<Query, IEnumerable<RankedDriver>>
    {
        public async Task<IEnumerable<RankedDriver>> Handle(Query query, CancellationToken cancellationToken)
        {
            return await ratingContext.Drivers
                .AsNoTracking()
                .Include(d => d.RankedBy)
                .Where(d => d.RankedBy.Id == query.UserContext.ProfileId)
                .ToListAsync(cancellationToken);
        }
    }

}