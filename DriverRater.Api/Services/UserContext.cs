namespace DriverRater.Api.Services;

using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.IdentityModel.JsonWebTokens;

public interface IUserContext
{
    string ExternalId { get; }
    string FirstName { get; }
    string LastName { get; }
    string? PictureUrl { get; }
    Guid? ProfileId { get; }
    string? Locale { get; }     
}

public class UserContext : IUserContext
{
    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User
                   ?? throw new InvalidOperationException("User is not authenticated so we can't access their info");

        ExternalId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
        FirstName = user.FindFirstValue(ClaimTypes.GivenName)!;
        LastName = user.FindFirstValue(ClaimTypes.Surname)!;
        PictureUrl = user.FindFirstValue("picture")!;
        Locale = user.FindFirstValue("locale");

        var driverIdClaim = user.FindFirstValue("profile_id");
        if (driverIdClaim is not null)
        {
            ProfileId = Guid.Parse(driverIdClaim);
        }
    }

    public string ExternalId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string? PictureUrl { get; }
    public Guid? ProfileId { get; }
    public string? Locale { get; }

    public bool IsRegistered => ProfileId != null;
}