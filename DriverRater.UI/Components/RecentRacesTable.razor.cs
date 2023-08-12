namespace DriverRater.UI.Components;

using DriverRater.Shared.RacingService.v1.Models;
using DriverRater.UI.Api;
using Microsoft.AspNetCore.Components;

public partial class RecentRacesTable : ComponentBase
{
    [Parameter]
    public int RacesForMemberId { get; set; }
    
    public List<RecentMemberRaceSummary>? Races { get; set; }

    [Inject]
    public IDriverRaterApi ApiClient { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        Races = (await ApiClient.GetRecentRaces(RacesForMemberId)).ToList();
    }
}