namespace DriverRater.Api.Features.Drivers.v1.Queries;

using DriverRater.Api.Entities;
using DriverRater.Api.Plumbing.Mediator;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

[UsedImplicitly]
public class GetDriversForUser
{
    [UsedImplicitly]
    public class Query : IQuery<IEnumerable<RankedDriver>>
    {
        public Guid UserId { get; init; }
    }

    [UsedImplicitly]
    public class QueryHandler : IQueryHandler<Query, IEnumerable<RankedDriver>>
    {
        private readonly DriverRatingContext ratingContext;
        
        public QueryHandler(DriverRatingContext ratingContext)
        {
            this.ratingContext = ratingContext;
        }
        
        public async Task<IEnumerable<RankedDriver>> Handle(Query query, CancellationToken cancellationToken)
        {
            // TODO: Support paging

            return await ratingContext.Drivers
                .AsNoTracking()
                .Include(d => d.RankedBy)
                .Where(d => d.RankedBy.Id == query.UserId)
                .ToListAsync(cancellationToken);
        }
    }

}