namespace DriverRater.Api.Features.RacingService.v1.Queries;

using Aydsko.iRacingData;
using DriverRater.Api.Exceptions;
using DriverRater.Api.Plumbing.Startup.Mediator;
using JetBrains.Annotations;

[UsedImplicitly]
public class GetMemberRecentRaces
{
    public record Query : IQuery<IEnumerable<Response>>
    {
        public int MemberId { get; set; }
    }

    public record Response
    {
        public int MemberId { get; set; }
        public int SubsessionId { get; set; }
        public string SeriesName { get; set; } = string.Empty;
        public int StartPosition { get; set; }
        public int FinishPosition { get; set; }
        public int Incidents { get; set; }
        public int StrengthOfField { get; set; }
        public string WinnerName { get; set; } = string.Empty;
        public DateTimeOffset SessionStartTime { get; set; }
    }

    [UsedImplicitly]
    public class QueryHandler(IDataClient client) : IQueryHandler<Query, IEnumerable<Response>>
    {
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