namespace DriverRater.Tests.Features.Drivers.v1;

using DriverRater.Entities;
using DriverRater.Features.Drivers.v1.Models;
using DriverRater.Tests.Plumbing;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions;

public class DriverControllerTests : ApiFactory<Startup>
{
    public DriverControllerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnDriversRatedByThisUser()
    {
        var user = new User("Rob", 59619);
        var context = GetService<DriverRatingContext>();
        var rating1 = new RankedDriver
        {
            Name = "Test Driver 1",
            Notes = "This guy is super fast",
            RacingId = 1,
            RankedBy = user
        };
        rating1.UpdateRank(DriverRank.Blue);
        var rating2 = new RankedDriver
        {
            Name = "Test Driver 2",
            Notes = "This guy is super wrecked me",
            RacingId = 2,
            RankedBy = user,
        };
        var rating3 = new RankedDriver
        {
            Name = "Test Driver 3",
            Notes = "This guy is fast and clean",
            RacingId = 2,
            RankedBy = new User("Test User", 100000),
        };
        rating2.UpdateRank(DriverRank.Green);
        context.Drivers.AddRange(rating1, rating2, rating3);
        await context.SaveChangesAsync();
        
        var client = CreateUnauthenticatedClient();

        var response = await client.Request($"/api/v1/driver/{user.Id}")
            .AllowAnyHttpStatus()
            .GetAsync();
        
        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        var data = await response.GetJsonAsync<IEnumerable<DriversRankModel>>();
        data.ShouldAllBe(d => d.UserId == user.Id);
        data.Count().ShouldBe(2);
    }

    [Fact]
    public async Task Rank_WhenDriverNotRankedByUser_ShouldRankDriver()
    {
        var context = GetService<DriverRatingContext>();
        
        var user = new User("Rob Gray", 59619);
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        var update = new UpdateDriverRankRequest
        {
            RankedDriverId = Guid.Empty,
            NewRank = (int)DriverRank.Black,
            RacingId = 59619,
            RankedByUserId = user.Id,
        };

        var client = CreateUnauthenticatedClient();
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
        
    }
}