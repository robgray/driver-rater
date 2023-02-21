namespace HelmetRanker.Entities;

public class User
{
    public User(string name, int racingId)
    {
        Id = Guid.NewGuid();
        Name = name;
        RacingId = racingId;
    }
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int RacingId { get; set; }

    public virtual ICollection<Driver> RankedDrivers { get; set; } = new List<Driver>();
}