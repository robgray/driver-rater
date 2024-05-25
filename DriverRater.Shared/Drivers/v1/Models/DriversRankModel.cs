namespace DriverRater.Shared.Drivers.v1.Models;

public class DriversRankModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int RacingId { get; set; }
    
    public Rank DriverRank { get; set; }
    
    public Guid UserId { get; set; }
 
    public DateTimeOffset? LastRankedDate { get; set; }
}