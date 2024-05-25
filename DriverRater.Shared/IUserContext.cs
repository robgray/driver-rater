namespace DriverRater.Shared;

public interface IUserContext
{
    string ExternalId { get; }
    string FirstName { get; }
    string LastName { get; }
    string? PictureUrl { get; }
    Guid? ProfileId { get; }
    string? Locale { get; }  
    string Name { get; }
    bool HasRegistered { get; }
}