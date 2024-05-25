<<<<<<<< HEAD:DriverRater.Shared/Models/RecentMemberRaceSummary.cs
﻿namespace DriverRater.Shared.Models;
========
﻿namespace DriverRater.Shared.RacingService.v1.Models;
>>>>>>>> 704296143f385a2951fb1d8d83d3c30ff9c3f554:DriverRater.Shared/RacingService/v1/Models/RecentMemberRaceSummary.cs

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