namespace DriverRater.UI;

using DriverRater.Shared.Models;

public interface IDriverRaterDataService
{
    public Task<RecentMemberRaceSummary[]> GetRecentRaces(int memberId, CancellationToken cancellationToken);
}