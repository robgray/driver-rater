namespace DriverRater.Features.Drivers.v1.Models;

using DriverRater.Entities;

public class DriversRankModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int RacingId { get; set; }
    public DriverRank DriverRank { get; set; }
    public Guid UserId { get; set; }
}