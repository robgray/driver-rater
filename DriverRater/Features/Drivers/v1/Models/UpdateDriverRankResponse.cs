namespace DriverRater.Features.Drivers.v1.Models;

using DriverRater.Entities;

public class UpdateDriverRankResponse
{
    public Guid RankedDriverId { get; set; }
    public string Name { get; set; }
    public int Rank { get; set; }
}