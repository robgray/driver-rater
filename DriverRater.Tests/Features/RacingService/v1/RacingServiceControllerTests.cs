namespace DriverRater.Tests.Features.RacingService;

using DriverRater.Entities;
using DriverRater.Features.RaceResults.v1.Models;
using DriverRater.Tests.Plumbing;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

public class RacingServiceControllerTests : ApiFactory<Startup>
{
    private const int MemberId = 59619; 
    
    public RacingServiceControllerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task RecentResults_ShouldReturnRecentMemberResults()
    {
        var client = CreateUnauthenticatedClient();

        var response = await client.Request($"/api/v1/racingservice/{MemberId}/recent")
            .AllowAnyHttpStatus()
            .GetAsync();

        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var data = await response.GetJsonAsync<IEnumerable<RecentMemberRaceSummary>>();
        data.ShouldAllBe(r => r.MemberId == MemberId);
        // This is live from iRacing so is subject to change.  It might even be empty if the driver hasn't raced in a while.  (Driver is me)
    }

    [Fact]
    public async Task GetRaceDrivers_WhenNoDriversRanked_ShouldGetAllDriversWithNoRank()
    {
        const int SubsessionId = 60565875;

        var user = new User("Rob Gray", 59619);
        var context = GetService<DriverRatingContext>();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        var client = CreateUnauthenticatedClient();
        var response = await client.Request($"/api/v1/racingservice/{SubsessionId}/drivers/{user.Id}")
            .AllowAnyHttpStatus()
            .GetAsync();

        var data = (await response.GetJsonAsync<IEnumerable<SubsessionRankedDriver>>()).ToArray();
        data.ShouldAllBe(d => d.Rank == DriverRank.None);
        data.Length.ShouldBe(12);
        
        data[0].DriverName.ShouldBe("Rob Gray");
        data[1].DriverName.ShouldBe("Sam Brooks7");
        data[2].DriverName.ShouldBe("Drazen Car");
        data[3].DriverName.ShouldBe("Mike DeGroot");
        data[4].DriverName.ShouldBe("David Babson");
        data[5].DriverName.ShouldBe("Mathues Guimaraes Jr.");
        data[6].DriverName.ShouldBe("Benjamin M Rogers");
        data[7].DriverName.ShouldBe("Tyrus Clements");
        data[8].DriverName.ShouldBe("Tray Law");
        data[9].DriverName.ShouldBe("Tsuyoshi Yoshinouchi");
        data[10].DriverName.ShouldBe("Randle Lahey");
        data[11].DriverName.ShouldBe("Robert Pegg");
    }
    
    [Fact]
    public async Task GetRaceDrivers_WhenSomeDriversRanked_ShouldGetAllDriversSomeRanked()
    {
        const int SubsessionId = 60565875;

        var user = new User("Rob Gray", 59619);
        var context = GetService<DriverRatingContext>();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var rankedDriver = new RankedDriver
        {
            Name = "Rob Gray",
            Notes = "Racing God",
            RacingId = 59619,
            RankedBy = user,
        };
        rankedDriver.UpdateRank(DriverRank.Blue);
        context.Drivers.Add(rankedDriver);
        await context.SaveChangesAsync();
        
        var client = CreateUnauthenticatedClient();
        var response = await client.Request($"/api/v1/racingservice/{SubsessionId}/drivers/{user.Id}")
            .AllowAnyHttpStatus()
            .GetAsync();

        var data = (await response.GetJsonAsync<IEnumerable<SubsessionRankedDriver>>()).ToArray();
        data.ShouldAllBe(d => d.Rank == DriverRank.None || d.Rank == DriverRank.Blue);
        data.Length.ShouldBe(12);
        
        data[0].DriverName.ShouldBe("Rob Gray");
        data[0].Rank.ShouldBe(DriverRank.Blue);
        
        data[1].DriverName.ShouldBe("Sam Brooks7");
        data[1].Rank.ShouldBe(DriverRank.None);
        data[2].DriverName.ShouldBe("Drazen Car");
        data[2].Rank.ShouldBe(DriverRank.None);
        data[3].DriverName.ShouldBe("Mike DeGroot");
        data[3].Rank.ShouldBe(DriverRank.None);
        data[4].DriverName.ShouldBe("David Babson");
        data[4].Rank.ShouldBe(DriverRank.None);
        data[5].DriverName.ShouldBe("Mathues Guimaraes Jr.");
        data[5].Rank.ShouldBe(DriverRank.None);
        data[6].DriverName.ShouldBe("Benjamin M Rogers");
        data[6].Rank.ShouldBe(DriverRank.None);
        data[7].DriverName.ShouldBe("Tyrus Clements");
        data[7].Rank.ShouldBe(DriverRank.None);
        data[8].DriverName.ShouldBe("Tray Law");
        data[8].Rank.ShouldBe(DriverRank.None);
        data[9].DriverName.ShouldBe("Tsuyoshi Yoshinouchi");
        data[9].Rank.ShouldBe(DriverRank.None);
        data[10].DriverName.ShouldBe("Randle Lahey");
        data[10].Rank.ShouldBe(DriverRank.None);
        data[11].DriverName.ShouldBe("Robert Pegg");
        data[11].Rank.ShouldBe(DriverRank.None);
    }
}