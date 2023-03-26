namespace DriverRater.Entities;

public class RankedDriver
{
    public RankedDriver()
    {
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; }
    public string Name { get; set; }
    public int RacingId { get; set; }
    public DriverRank Rank { get; private set; }
    public User RankedBy { get; init; }
    public DateTime DateRankedUtc { get; private set; }
    
    public string Notes { get; set; }

    public void UpdateRank(DriverRank rank)
    {
        Rank = rank;
        DateRankedUtc = DateTime.UtcNow;
    }
}