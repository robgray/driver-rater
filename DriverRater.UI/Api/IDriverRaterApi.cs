namespace DriverRater.UI.Api;

using DriverRater.Shared.Drivers.v1.Models;
using DriverRater.Shared.RacingService.v1.Models;

public interface IDriverRaterApi
{
    Task<IEnumerable<RecentMemberRaceSummary>> GetRecentRaces(int customerId);

    Task<IEnumerable<DriversRankModel>> GetDriversInRace(int subsessionId);
    
    string DownloadHelmetPackForUserUrl();

    string DownloadHelmetPackForAllUrl();

    Task<IEnumerable<DriversRankModel>> GetDriversForUser();

    Task<UpdateDriverRankResponse> UpdateDriverRankResponse(UpdateDriverRankRequest request);
}