namespace DriverRater.UI.Components;

using DriverRater.Shared.RacingService.v1.Models;
using Microsoft.AspNetCore.Components;

public partial class RaceInfoPanel
{
    [Parameter]
    public RecentMemberRaceSummary? RaceSummary { get; set; }
}