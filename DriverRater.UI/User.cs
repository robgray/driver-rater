namespace DriverRater.UI;

using System.Security.Claims;
using DriverRater.Shared;
using Microsoft.AspNetCore.Components.Authorization;

public class User : IUserContext
{
    private User(string externalId, string firstName, string lastName)
    {
        ExternalId = externalId;
        FirstName = firstName;
        LastName = lastName;
    }

    public static async Task<IUserContext> GetUser(AuthenticationStateProvider stateProvider)
    {
        var authState = await stateProvider.GetAuthenticationStateAsync();
        var authUser = authState.User;

        if (!authUser.Claims.Any())
        {
            throw new InvalidOperationException("User is not authenticated so we can't access their info");
        }
        
        var externalId = authUser.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("External Id claim is missing");
        var firstName = authUser.FindFirst(ClaimTypes.GivenName)
            ?? throw new InvalidOperationException("Given Name claim is missing");
        var lastName = authUser.FindFirst(ClaimTypes.Surname)
            ?? throw new InvalidOperationException("Surname claim is missing");
        
        var user = new User(externalId!.Value, firstName!.Value, lastName!.Value);
        user.PictureUrl = authUser.FindFirst("picture")?.Value ?? null;
        user.Locale = authUser.FindFirst("locale")?.Value ?? null;

        var driverIdClaim =  authUser.FindFirst("profile_id");
        if (driverIdClaim is not null)
        {
            user.ProfileId = Guid.Parse(driverIdClaim.Value);
        }

        return user;
    }
    
    public string ExternalId { get; }
    
    public string FirstName { get; }
    
    public string LastName { get; }
    
    public string? PictureUrl { get; private set;  }
    
    public Guid? ProfileId { get; private set; } = Guid.Parse("982F658B-F81C-409A-956B-6BEDB4CA3F3D");
    
    public string? Locale { get; private set; }

    public string Name => $"{FirstName} {LastName}";

    public bool HasRegistered => ProfileId != null;
}