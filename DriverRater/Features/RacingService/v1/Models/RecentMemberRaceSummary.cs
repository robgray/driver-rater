namespace DriverRater.Features.RaceResults.v1.Models;

public class RecentMemberRaceSummary
{   
    public int MemberId { get; set; }
    public int SubsessionId { get; set; }
    public string SeriesName { get; set; }
    public int StartPosition { get; set; }
    public int FinishPosition { get; set; }
    public int Incidents { get; set; }
    public int StrengthOfField { get; set; }
    public string WinnerName { get; set; }
    public DateTimeOffset SessionStartTime { get; set; }
}