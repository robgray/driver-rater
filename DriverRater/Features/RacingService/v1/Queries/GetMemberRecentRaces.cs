namespace DriverRater.Features.RaceResults.v1.Queries;

using Aydsko.iRacingData;
using DriverRater.Exceptions;
using DriverRater.Features.RaceResults.v1.Models;
using DriverRater.Plumbing.Mediator;
using JetBrains.Annotations;

[UsedImplicitly]
public class GetMemberRecentRaces
{
    public class Query : IQuery<IEnumerable<Response>>
    {
        public int MemberId { get; set; }
    }

    public class Response
    {
        public int MemberId { get; set; }
        public int SubsessionId { get; set; }
        public string SeriesName { get; set; }
        public int StartPosition { get; set; }
        public int FinishPosition { get; set; }
        public int Incidents { get; set; }
        public int StrengthOfField { get; set; }
        public string WinnerName { get; set; }
        public DateTimeOffset SessionStartTime { get; set; }
    }

    [UsedImplicitly]
    public class QueryHandler : IQueryHandler<Query, IEnumerable<Response>>
    {
        private readonly IDataClient client;
        
        public QueryHandler(IDataClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Response>> Handle(Query query, CancellationToken cancellationToken)
        {
            var response = await client.GetMemberRecentRacesAsync(query.MemberId, cancellationToken);

            if (!response.Data.Races.Any())
            {
                throw new EntityNotFoundException("No races exist");
            }

            return response.Data.Races.Select(r => new Response
            {
                MemberId = query.MemberId,
                SubsessionId = r.SubsessionId,
                Incidents = r.Incidents,
                StartPosition = r.StartPosition,
                FinishPosition = r.FinishPosition,
                SeriesName = r.SeriesName,
                WinnerName = r.WinnerName,
                StrengthOfField = r.StrengthOfField,
                SessionStartTime = r.SessionStartTime,
            });
        }
    }
}