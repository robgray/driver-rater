namespace DriverRater.Tests.Features.Helmets.v1;

using DriverRater.Api;
using DriverRater.Api.Entities;
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
        var profile = new Profile("Rob Gray", 59619);
        context.Profiles.Add(profile);
        await context.SaveChangesAsync(ct);

        var ranked1 =new RankedDriver(profile)
        {
            Name = "Gianni Lutzu",
            RacingId = 2345,
        };
        ranked1.UpdateRank(DriverRank.Blue);

        var ranked2 = new RankedDriver(profile)
        {
            Name = "Drazen Car",
            RacingId = 34556,
        };
        ranked2.UpdateRank(DriverRank.Green);
        var ranked3 = new RankedDriver(profile)
        {
            Name = "Sam Brookes",
            RacingId = 345426,
        };
        ranked3.UpdateRank(DriverRank.Yellow);
        var ranked4 = new RankedDriver(profile)
        {
            Name = "Mark Dracup",
            RacingId = 15426,
        };
        ranked4.UpdateRank(DriverRank.Red);
        var ranked5 = new RankedDriver(profile)
        {
            Name = "Jeremy Fletcher",
            RacingId = 666,
        };
        ranked5.UpdateRank(DriverRank.Black);        
        context.Drivers.AddRange(ranked1, ranked2, ranked3, ranked4, ranked5);
        await context.SaveChangesAsync(ct);

        var client = CreateAuthenticatedClient(profile.Id);
        var response = await client.Request($"/api/v1/helmetpack")
            .AllowAnyHttpStatus()
            .DownloadFileAsync(
                localFolderPath: "C:/temp/",
                localFileName: $"helmet-{profile.RacingId}.zip",
                cancellationToken: ct);

        response.ShouldBe($"C:/temp/helmet-{profile.RacingId}.zip");
    }
}