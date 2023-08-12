namespace DriverRater.Api.Features.RacingService.v1.Queries;

using Aydsko.iRacingData;
using DriverRater.Api.Entities;
using DriverRater.Api.Exceptions;
using DriverRater.Api.Plumbing.Mediator;
using Microsoft.EntityFrameworkCore;
using SessionTypes = DriverRater.Api.Helpers.SessionTypes;

public class GetRacersInSubsession
{
    public class Query : IQuery<IEnumerable<Response>>
    {
        public Guid UserId { get; set; }
        public int SubsessionId { get; set; }
    }

    public class Response
    {
        public Guid RankedDriverId { get; set; }
        public int MemberId { get; set; }
        public string DriverName { get; set; }
        public DriverRank Rank { get; set; }
        
        public Guid RankedByUserId { get; set; }
        
        public DateTimeOffset? LastRankedDate { get; set; }
    }
    
    public class QueryHandler : IQueryHandler<Query, IEnumerable<Response>>
    {
        private readonly IDataClient client;
        private readonly DriverRatingContext context;

        public QueryHandler(IDataClient client, DriverRatingContext context)
        {
            this.client = client;
            this.context = context;
        }
        
        public async Task<IEnumerable<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = context.Profiles.SingleOrDefault(x => x.Id == query.UserId)
                       ?? throw new EntityNotFoundException("User could not be found");
            
            var result = await client.GetSubSessionResultAsync(query.SubsessionId, includeLicenses: false, cancellationToken);
            
            var raceResults = result.Data.SessionResults[SessionTypes.Race].Results
                .Where(r => r.CustomerId != user.RacingId)
                .Select(r => new Response
            {
                MemberId = r.CustomerId??0,
                DriverName = r.DisplayName,
            }).ToList();

            var memberIds = raceResults.Select(r => r.MemberId).ToArray();
            
            // Find Driver rank for user.
            var rankedDrivers = await context.Drivers
                .AsNoTracking()
                .Include(d => d.RankedBy)
                .Where(d => d.RankedBy.Id == query.UserId && memberIds.Contains(d.RacingId))
                .ToListAsync(cancellationToken);

            foreach (var rankedDriver in rankedDrivers)
            {
                var raceResult = raceResults.Single(r => r.MemberId == rankedDriver.RacingId);
                raceResult.Rank = rankedDriver.Rank;
                raceResult.RankedDriverId = rankedDriver.Id;
                raceResult.LastRankedDate = rankedDriver.DateRanked;
                raceResult.RankedByUserId = rankedDriver.RankedBy.Id;
            }

            return raceResults;
        }  
    }
}
