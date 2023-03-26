namespace DriverRater.Features.Drivers.v1.Models;

using System.ComponentModel.DataAnnotations;

public class NewDriverRankRequest
{
    [Required]
    public string Name { get; init; }
    
    [Range(0, Int32.MaxValue)]
    public int RacingId { get; init; }
    
    [Range(0, Int32.MaxValue)]
    public int Rank { get; set; }
}