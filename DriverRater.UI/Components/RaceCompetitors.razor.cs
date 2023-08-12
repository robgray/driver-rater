namespace DriverRater.UI.Components;

using DriverRater.Shared;
using DriverRater.Shared.Drivers.v1.Models;
using DriverRater.Shared.RacingService.v1.Models;
using DriverRater.UI.Api;
using Microsoft.AspNetCore.Components;

public partial class RaceCompetitors : ComponentBase
{
    [Parameter]
    public int? SubsessionId { get; set; }
    
    [Inject]
    public IDriverRaterApi Client { get; set; }
    
    public List<DriversRankModel>? Drivers { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Drivers = SubsessionId.HasValue
                ? (await Client.GetDriversInRace(SubsessionId.Value)).ToList()
                : (await Client.GetDriversForUser()).OrderBy(d => d.Name).ToList();
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }

    protected async Task RankChanged(ChangeEventArgs e, DriversRankModel driver)
    {
        var request = new UpdateDriverRankRequest()
        {
            NewRank = e.Value switch
            {
                "1" => Rank.Blue,
                "2" => Rank.Green,
                "3" => Rank.Yellow,
                "4" => Rank.Red,
                "5" => Rank.Black,
                _ => Rank.None,
            }, 
            RacingId = driver.RacingId,
            RankedByUserId = Guid.Parse("982F658B-F81C-409A-956B-6BEDB4CA3F3D"),
            RankedDriverId = driver.Id,
        };
        var response = await Client.UpdateDriverRankResponse(request);
        var updatedDriver = Drivers!.Single(x => x.Id == response.RankedDriverId);
        updatedDriver.Id = response.RankedDriverId;
        updatedDriver.DriverRank = response.Rank;
        updatedDriver.LastRankedDate = DateTimeOffset.Now;
    }
}