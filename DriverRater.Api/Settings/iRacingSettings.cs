namespace DriverRater.Api.Settings;

using FluentValidation;

public class iRacingSettings
{
    public const string Key = "iRacing";

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class iRacingSettingsValidator : AbstractValidator<iRacingSettings>
{
    public iRacingSettingsValidator()
    {
        RuleFor(x => x.Username).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}