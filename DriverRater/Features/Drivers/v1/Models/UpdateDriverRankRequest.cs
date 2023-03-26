namespace DriverRater.Features.Drivers.v1.Models;

using System.ComponentModel.DataAnnotations;

public class UpdateDriverRankRequest
{
    public Guid RankedDriverId { get; set; }
    
    public int RacingId { get; set; }
    
    [Range(0, int.MaxValue)]
    public int NewRank { get; set; }
    
    public Guid RankedByUserId { get; set; }
}