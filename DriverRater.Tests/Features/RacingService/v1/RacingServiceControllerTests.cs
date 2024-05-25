namespace DriverRater.Tests.Features.RacingService;

using DriverRater.Api;
using DriverRater.Api.Entities;
using DriverRater.Shared;
using DriverRater.Shared.Drivers.v1.Models;
using DriverRater.Shared.RacingService.v1.Models;
using DriverRater.Tests.Plumbing;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

public class RacingServiceControllerTests : ApiFactory<Startup>
{
    private const int MemberId = 59619;
    private readonly Profile profile;
    
    public RacingServiceControllerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        profile = new Profile("Rob Gray", 59619);
        var context = GetService<DriverRatingContext>();
        
        context.Profiles.Add(profile);
        context.SaveChanges();
    }

    [Fact]
    public async Task RecentResults_ShouldReturnRecentMemberResults()
    {
        var client = CreateAuthenticatedClient(profile.Id);

        var response = await client.Request($"/api/v1/racingservice/{MemberId}/recent")
            .AllowAnyHttpStatus()
            .GetAsync();

        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var data = await response.GetJsonAsync<IEnumerable<RecentMemberRaceSummary>>();
        data.ShouldAllBe(r => r.MemberId == MemberId);
        // This is live from iRacing so is subject to change.  It might even be empty if the driver hasn't raced in a while.  (Driver is me)
    }

    [Fact]
    public async Task GetRaceDrivers_WhenNoDriversRanked_ShouldGetAllDriversButSelfWithNoRank()
    {
        // This Subsession has Rob Gray in it and he is a user.
        const int SubsessionId = 60565875;

        var client = CreateAuthenticatedClient(profile.Id);
        var response = await client.Request($"/api/v1/racingservice/{SubsessionId}/drivers")
            .AllowAnyHttpStatus()
            .GetAsync();

        var data = (await response.GetJsonAsync<IEnumerable<DriversRankModel>>()).ToArray();
        data.ShouldAllBe(d => d.DriverRank == Rank.None);
        data.Length.ShouldBe(11);
        
        data[0].Name.ShouldBe("Sam Brooks7");
        data[1].Name.ShouldBe("Drazen Car");
        data[2].Name.ShouldBe("Mike DeGroot");
        data[3].Name.ShouldBe("David Babson");
        data[4].Name.ShouldBe("Matheus Guimaraes Jr");
        data[5].Name.ShouldBe("Benjamin M Rogers");
        data[6].Name.ShouldBe("Tyrus Clements");
        data[7].Name.ShouldBe("Tray Law");
        data[8].Name.ShouldBe("Tsuyoshi Yoshinouchi");
        data[9].Name.ShouldBe("Randle Lahey");
        data[10].Name.ShouldBe("Robert Pegg");
    }
    
    [Fact]
    public async Task GetRaceDrivers_WhenNoDriversRanked_ShouldGetAllDriversWithNoRank()
    {
        // This Subsession has Rob Gray in it and he is a user.
        const int SubsessionId = 60565875;
        
        var notMeProfile = new Profile("Not Me", 1);
        var context = GetService<DriverRatingContext>(); 
        context.Profiles.Add(notMeProfile);
        await context.SaveChangesAsync();
        
        var client = CreateAuthenticatedClient(notMeProfile.Id);
        var response = await client.Request($"/api/v1/racingservice/{SubsessionId}/drivers")
            .AllowAnyHttpStatus()
            .GetAsync();

        var data = (await response.GetJsonAsync<IEnumerable<DriversRankModel>>()).ToArray();
        data.ShouldAllBe(d => d.DriverRank == Rank.None);
        data.Length.ShouldBe(12);
        
        data[0].Name.ShouldBe("Rob Gray");
        data[1].Name.ShouldBe("Sam Brooks7");
        data[2].Name.ShouldBe("Drazen Car");
        data[3].Name.ShouldBe("Mike DeGroot");
        data[4].Name.ShouldBe("David Babson");
        data[5].Name.ShouldBe("Matheus Guimaraes Jr");
        data[6].Name.ShouldBe("Benjamin M Rogers");
        data[7].Name.ShouldBe("Tyrus Clements");
        data[8].Name.ShouldBe("Tray Law");
        data[9].Name.ShouldBe("Tsuyoshi Yoshinouchi");
        data[10].Name.ShouldBe("Randle Lahey");
        data[11].Name.ShouldBe("Robert Pegg");
    }
    
    [Fact]
    public async Task GetRaceDrivers_WhenSomeDriversRanked_ShouldGetAllDriversSomeRanked()
    {
        const int SubsessionId = 60565875;
        var notMeProfile = new Profile("Not Me", 1);
        var context = GetService<DriverRatingContext>(); 
        context.Profiles.Add(notMeProfile);
        await context.SaveChangesAsync();

        var rankedDriver = new RankedDriver(notMeProfile)
        {
            Name = "Rob Gray",
            Notes = "Racing God",
            RacingId = 59619,
        };
        rankedDriver.UpdateRank(DriverRank.Blue);
        context.Drivers.Add(rankedDriver);
        await context.SaveChangesAsync();
        
        var client = CreateAuthenticatedClient(notMeProfile.Id);
        var response = await client.Request($"/api/v1/racingservice/{SubsessionId}/drivers")
            .AllowAnyHttpStatus()
            .GetAsync();

        var data = (await response.GetJsonAsync<IEnumerable<DriversRankModel>>()).ToArray();
        data.ShouldAllBe(d => d.DriverRank == Rank.None || d.DriverRank == Rank.Blue);
        data.Length.ShouldBe(12);
        
        data[0].Name.ShouldBe("Rob Gray");
        data[0].DriverRank.ShouldBe(Rank.Blue);
        
        data[1].Name.ShouldBe("Sam Brooks7");
        data[1].DriverRank.ShouldBe(Rank.None);
        data[2].Name.ShouldBe("Drazen Car");
        data[2].DriverRank.ShouldBe(Rank.None);
        data[3].Name.ShouldBe("Mike DeGroot");
        data[3].DriverRank.ShouldBe(Rank.None);
        data[4].Name.ShouldBe("David Babson");
        data[4].DriverRank.ShouldBe(Rank.None);
        data[5].Name.ShouldBe("Matheus Guimaraes Jr");
        data[5].DriverRank.ShouldBe(Rank.None);
        data[6].Name.ShouldBe("Benjamin M Rogers");
        data[6].DriverRank.ShouldBe(Rank.None);
        data[7].Name.ShouldBe("Tyrus Clements");
        data[7].DriverRank.ShouldBe(Rank.None);
        data[8].Name.ShouldBe("Tray Law");
        data[8].DriverRank.ShouldBe(Rank.None);
        data[9].Name.ShouldBe("Tsuyoshi Yoshinouchi");
        data[9].DriverRank.ShouldBe(Rank.None);
        data[10].Name.ShouldBe("Randle Lahey");
        data[10].DriverRank.ShouldBe(Rank.None);
        data[11].Name.ShouldBe("Robert Pegg");
        data[11].DriverRank.ShouldBe(Rank.None);
    }
}