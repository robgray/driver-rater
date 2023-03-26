namespace DriverRater.Features.RaceResults.v1.Models;

using DriverRater.Entities;

public class SubsessionRankedDriver
{
    public Guid RankedDriverId { get; set; }
    public int MemberId { get; set; }
    public string DriverName { get; set; }
    public DriverRank Rank { get; set; }
}