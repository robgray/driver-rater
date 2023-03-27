namespace DriverRater.Tests.Features.Helmets.v1;

using DriverRater.Entities;
using DriverRater.Tests.Plumbing;
using Flurl.Http;
using Xunit.Abstractions;

public class HelmetPackControllerTests : ApiFactory<Startup>
{
    private CancellationToken ct = new(false);
    
    public HelmetPackControllerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task DownloadHelmetPackForUser_ShouldDownloadAllRankedDriversForUser()
    {
        var context = GetService<DriverRatingContext>();
        var user = new User("Rob Gray", 59619);
        context.Users.Add(user);
        await context.SaveChangesAsync(ct);

        var ranked1 =new RankedDriver
        {
            Name = "Gianni Lutzu",
            RacingId = 2345,
            RankedBy = user,
        };
        ranked1.UpdateRank(DriverRank.Blue);

        var ranked2 = new RankedDriver
        {
            Name = "Drazen Car",
            RacingId = 34556,
            RankedBy = user,
        };
        ranked2.UpdateRank(DriverRank.Green);
        var ranked3 = new RankedDriver
        {
            Name = "Sam Brookes",
            RacingId = 345426,
            RankedBy = user,
        };
        ranked3.UpdateRank(DriverRank.Yellow);
        var ranked4 = new RankedDriver
        {
            Name = "Mark Dracup",
            RacingId = 15426,
            RankedBy = user,
        };
        ranked4.UpdateRank(DriverRank.Red);
        var ranked5 = new RankedDriver
        {
            Name = "Jeremy Fletcher",
            RacingId = 666,
            RankedBy = user,
        };
        ranked5.UpdateRank(DriverRank.Black);        
        context.Drivers.AddRange(ranked1, ranked2, ranked3, ranked4, ranked5);
        await context.SaveChangesAsync(ct);

        var client = CreateUnauthenticatedClient();
        var response = await client.Request($"/api/v1/helmetpack/{user.Id}")
            .AllowAnyHttpStatus()
            .DownloadFileAsync(
                localFolderPath: "C:/temp/",
                localFileName: $"helmet-{user.Id}",
                cancellationToken: ct);

        response.ShouldBe($"C:/temp/helmet-{user.Id}");
    }
}