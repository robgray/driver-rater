namespace DriverRater.Api;

using Aydsko.iRacingData;
using DriverRater.Api.Plumbing.Options;
using DriverRater.Api.Plumbing.Startup.Auth;
using DriverRater.Api.Plumbing.Startup.Automapper;
using DriverRater.Api.Plumbing.Startup.Controllers;
using DriverRater.Api.Plumbing.Startup.Cors;
using DriverRater.Api.Plumbing.Startup.DbContext;
using DriverRater.Api.Plumbing.Startup.Mapster;
using DriverRater.Api.Plumbing.Startup.Mediator;
using DriverRater.Api.Plumbing.Startup.Swagger;
using DriverRater.Api.Plumbing.Startup.UserContext;
using DriverRater.Api.Plumbing.Startup.Validation;
using DriverRater.Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Serilog;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        Env = env;
    }
    
    public IConfiguration Configuration { get; }

    public IWebHostEnvironment Env { get; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        IdentityModelEventSource.ShowPII = Env.IsDevelopment();

        services
            .AddHttpContextAccessor()
            .AddCustomValidation()
            .AddCustomCors(Configuration)
            .AddCustomControllers()
            .AddCustomAuth(Configuration)
            .AddCustomSwagger()
            .AddCustomMapster()
            .AddCustomMediator()
            .AddCustomDbContext(Configuration)
            .AddCustomUserContext()
            .AddCustomOptions(Configuration);

        services.AddHealthChecks();

        var iracingOptions = Configuration
            .GetSection(iRacingSettings.Key)
            .Get<iRacingSettings>();

        if (iracingOptions is null)
        {
            Log.Fatal("iRacing API has not been configured");
            throw new ApplicationException();
        }

        services.AddIRacingDataApi(options =>
        {
            options.UserAgentProductName = "Driver Ranker";
            options.UserAgentProductVersion = new Version(1, 0);
            options.Username = iracingOptions.Username;
            options.Password = iracingOptions.Password;
        });
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IOptionsMonitor<AuthenticationSettings> authenticationOptionsMon)
    {
        // Configure the HTTP request pipeline.
        if (!env.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseCustomSwagger(Configuration);
        }

        if (env.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }

        app.UseBlazorFrameworkFiles();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCustomAuth();
        app.UseCors("Open");
        app.UseSerilogRequestLogging();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            //endpoints.Map("api/{**slug}", HandleApiFallback);
            endpoints.MapFallbackToFile("{**slug}", "index.html");
        });

        /*
        Task HandleApiFallback(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return Task.CompletedTask;
        }
        */
    }
}