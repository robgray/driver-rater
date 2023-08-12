namespace DriverRater.Api.Options;

public class Auth0Options
{
    public const string Key = "Auth0";
    
    public string Domain { get; set; }
    
    public string Audience { get; set; }
}