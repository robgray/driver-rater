namespace DriverRater.Api.Entities;

public class Profile(string name, int racingId)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public int RacingId { get; set; } = racingId;

    public virtual ICollection<RankedDriver> RankedDrivers { get; set; } = new List<RankedDriver>();
}