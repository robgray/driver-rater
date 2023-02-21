namespace HelmetRanker.Features.Drivers.v1.Queries;

using HelmetRanker.Entities;
using HelmetRanker.Plumbing.Mediator;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

[UsedImplicitly]
public class GetDriversForUser
{
    [UsedImplicitly]
    public class Query : IQuery<IEnumerable<Driver>>
    {
        public Guid UserId { get; init; }
    }

    [UsedImplicitly]
    public class QueryHandler : IQueryHandler<Query, IEnumerable<Driver>>
    {
        private readonly DriverContext context;
        
        public QueryHandler(DriverContext context)
        {
            this.context = context;
        }
        
        public async Task<IEnumerable<Driver>> Handle(Query query, CancellationToken cancellationToken)
        {
            // TODO: Support paging
            
            return await context.Drivers
                .AsNoTracking()
                .Where(d => d.RankedBy.Id == query.UserId)
                .ToListAsync(cancellationToken);
        }
    }

}