namespace DriverRater.UI;

using DriverRater.Shared.RacingService.v1.Models;
using Flurl.Http;

public class DriverRaterDataService(FlurlClient client) : IDriverRaterDataService
{
    public async Task<RecentMemberRaceSummary[]> GetRecentRaces(int memberId, CancellationToken cancellationToken)
    {
        var response = await client.Request($"/api/v1/racingservice/{memberId}/recent")
            .GetAsync(cancellationToken: cancellationToken);

        var data = await response.GetJsonAsync<RecentMemberRaceSummary[]>();
        return data;
    }
    
}
