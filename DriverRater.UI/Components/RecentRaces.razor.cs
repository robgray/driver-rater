namespace DriverRater.UI.Components;

using DriverRater.Shared.Models;
using Microsoft.AspNetCore.Components;

public partial class RecentRaces
{
    [Parameter]
    public int MemberId { get; set; }
    
    [Inject]
    public IDriverRaterDataService DataService { get; set; }

    public RecentMemberRaceSummary[]? RaceSummaries { get; set; }

    protected override async Task OnInitializedAsync()
    {
        RaceSummaries = await DataService.GetRecentRaces(MemberId, new CancellationToken(false));
    }
}
