namespace DriverRater.Shared.Drivers.v1.Models;

public class UpdateDriverRankRequest
{
    public Guid RankedDriverId { get; set; }
    
    public int RacingId { get; set; }
    
    public Rank NewRank { get; set; }
    
    public Guid RankedByUserId { get; set; }
}