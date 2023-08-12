namespace DriverRater.Tests.Features.Drivers.v1;

using DriverRater.Api;
using DriverRater.Api.Entities;
using DriverRater.Shared;
using DriverRater.Shared.Drivers.v1.Models;
using DriverRater.Tests.Plumbing;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

public class DriverControllerTests : ApiFactory<Startup>
{
    public DriverControllerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnDriversRatedByThisUser()
    {
        var (profile,context) = await CreateTestProfile();
        var rating1 = new RankedDriver(profile)
        {
            Name = "Test Driver 1",
            Notes = "This guy is super fast",
            RacingId = 1,
        };
        rating1.UpdateRank(DriverRank.Blue);
        var rating2 = new RankedDriver(profile)
        {
            Name = "Test Driver 2",
            Notes = "This guy is super wrecked me",
            RacingId = 2,
        };
        var rating3 = new RankedDriver(new Profile("Test User", 100000))
        {
            Name = "Test Driver 3",
            Notes = "This guy is fast and clean",
            RacingId = 2,
        };
        rating2.UpdateRank(DriverRank.Green);
        context.Drivers.AddRange(rating1, rating2, rating3);
        await context.SaveChangesAsync();
        
        var client = CreateAuthenticatedClient(profile.Id);

        var response = await client.Request($"/api/v1/driver/{profile.Id}")
            .AllowAnyHttpStatus()
            .GetAsync();
        
        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var data = (await response.GetJsonAsync<IEnumerable<DriversRankModel>>()).ToList();
        data.ShouldAllBe(d => d.UserId == profile.Id);
        data.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Rank_WhenDriverNotRankedByUser_ShouldRankDriver()
    {
        var (user,context) = await CreateTestProfile();

        var update = new UpdateDriverRankRequest
        {
            RankedDriverId = Guid.Empty,
            NewRank = Rank.Black,
            RacingId = 59619,
            RankedByUserId = user.Id,
        };

        var client = CreateAuthenticatedClient(user.Id);
        var response = await client.Request($"/api/v1/driver/rank")
            .AllowAnyHttpStatus()
            .PostJsonAsync(update);

        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var data = await response.GetJsonAsync<UpdateDriverRankResponse>();

        var rankedDriver = context.Drivers.SingleOrDefault(d => d.Id == data.RankedDriverId);
        rankedDriver.ShouldNotBeNull();
        
        rankedDriver.RacingId.ShouldBe(59619);
        rankedDriver.RankedBy.Id.ShouldBe(user.Id);
        rankedDriver.Name.ShouldBe("Rob Gray");
        rankedDriver.Rank.ShouldBe(DriverRank.Black);
    }
    
    [Fact]
    public async Task Rank_WhenDriverRankedByUser_ShouldUpdatedRank()
    {
        var (user,context) = await CreateTestProfile();
        
        var rating1 = new RankedDriver(user)
        {
            Name = "Rob Gray",
            Notes = "This guy is super fast",
            RacingId = 59619,
        };
        rating1.UpdateRank(DriverRank.Blue);
        context.Drivers.Add(rating1);
        await context.SaveChangesAsync();
        
        var update = new UpdateDriverRankRequest
        {
            RankedDriverId = Guid.Empty,
            NewRank = Rank.Black,
            RacingId = 59619,
            RankedByUserId = user.Id,
        };

        var client = CreateAuthenticatedClient(user.Id);
        var response = await client.Request($"/api/v1/driver/rank")
            .AllowAnyHttpStatus()
            .PostJsonAsync(update);

        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var data = await response.GetJsonAsync<UpdateDriverRankResponse>();

        var rankedDriver = context.Drivers
            .Include(rankedDriver => rankedDriver.RankedBy)
            .SingleOrDefault(d => d.Id == data.RankedDriverId);
        rankedDriver.ShouldNotBeNull();
        
        rankedDriver.RacingId.ShouldBe(59619);
        rankedDriver.RankedBy.Id.ShouldBe(user.Id);
        rankedDriver.Name.ShouldBe("Rob Gray");
        rankedDriver.Rank.ShouldBe(DriverRank.Black);
    }

    private async Task<(Profile,DriverRatingContext)> CreateTestProfile()
    {
        var context = GetService<DriverRatingContext>();
        
        var user = new Profile("Rob Gray", 59619);
        context.Profiles.Add(user);
        await context.SaveChangesAsync();

        return (user,context);
    }
}