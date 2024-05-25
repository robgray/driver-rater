namespace DriverRater.Api.Settings;

using System.ComponentModel.DataAnnotations;
using FluentValidation;

public class AuthenticationSettings
{
    public const string Key = "Authentication";
    
    public string Authority { get; init; } = string.Empty;
    
    public string Audience { get; init; } = string.Empty;
}

public sealed class AuthenticationSettingsValidator : AbstractValidator<AuthenticationSettings>
{
    public AuthenticationSettingsValidator()
    {
        RuleFor(x => x.Authority)
            .NotEmpty()
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));
        
        RuleFor(x => x.Audience).NotEmpty();
    }
}