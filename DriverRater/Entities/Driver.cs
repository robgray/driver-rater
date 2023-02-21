namespace HelmetRanker.Entities;

public class Driver
{
    public Driver(string name, int racingId, DriverRank rank, User rankedBy)
    {
        Id = Guid.NewGuid();
        Name = name;
        RacingId = racingId;
        Rank = rank;
        RankedBy = rankedBy;
        DateRankedUtc = DateTime.UtcNow;
    }
    
    public Guid Id { get; init; }
    public string Name { get; set; }
    public int RacingId { get; set; }
    public DriverRank Rank { get; private set; }
    public User RankedBy { get; init; }
    public DateTime DateRankedUtc { get; private set; }

    public void UpdateRank(DriverRank rank)
    {
        Rank = rank;
        DateRankedUtc = DateTime.UtcNow;
    }
}