namespace DriverRater.UI;

using DriverRater.Shared.RacingService.v1.Models;

public interface IDriverRaterDataService
{
    public Task<RecentMemberRaceSummary[]> GetRecentRaces(int memberId, CancellationToken cancellationToken);
}