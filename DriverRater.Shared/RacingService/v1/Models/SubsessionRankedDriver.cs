namespace DriverRater.Shared.RacingService.v1.Models;

public class SubsessionRankedDriver
{
    public Guid RankedDriverId { get; set; }
    public int MemberId { get; set; }
    public string DriverName { get; set; }
    public Rank Rank { get; set; }
    
    public DateTimeOffset? LastRankedDate { get; set; }
}