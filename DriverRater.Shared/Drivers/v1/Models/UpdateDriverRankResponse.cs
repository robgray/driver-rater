namespace DriverRater.Shared.Drivers.v1.Models;

public class UpdateDriverRankResponse
{
    public Guid RankedDriverId { get; set; }
    public string Name { get; set; }
    public Rank Rank { get; set; }
}