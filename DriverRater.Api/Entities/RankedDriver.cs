namespace DriverRater.Api.Entities;

public class RankedDriver
{
    public RankedDriver(Profile rankingProfile)
    {
        RankedBy = rankingProfile;
    }

    private RankedDriver()
    {
    }
    
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int RacingId { get; set; }
    public DriverRank Rank { get; private set; }
    public Profile RankedBy { get; init; }
    public DateTimeOffset DateRanked { get; private set; }
    
    public string? Notes { get; set; }

    public void UpdateRank(DriverRank rank)
    {
        Rank = rank;
        DateRanked = DateTimeOffset.Now;
    }
}