namespace DriverRater.Api.Entities;

public class Profile
{
    public Profile(string name, int racingId)
    {
        Id = Guid.NewGuid();
        Name = name;
        RacingId = racingId;
    }
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int RacingId { get; set; }

    public virtual ICollection<RankedDriver> RankedDrivers { get; set; } = new List<RankedDriver>();
}