namespace DriverRater.Tests.Plumbing;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
{
    public void PostConfigure(string name, JwtBearerOptions options)
    {
        options.Authority = null;
        options.TokenValidationParameters.ValidateIssuer = false;
        options.TokenValidationParameters.ValidateIssuerSigningKey = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.TestSecurityKey));
    }
}