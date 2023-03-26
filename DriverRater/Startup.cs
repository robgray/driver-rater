namespace DriverRater;

using Aydsko.iRacingData;
using DriverRater.Options;
using DriverRater.Plumbing.Automapper;
using DriverRater.Plumbing.Controllers;
using DriverRater.Plumbing.Cors;
using DriverRater.Plumbing.DbContext;
using DriverRater.Plumbing.Mediator;
using DriverRater.Plumbing.Options;
using DriverRater.Plumbing.Swagger;
using Microsoft.IdentityModel.Logging;

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

        services.AddCustomCors(Configuration)
            .AddCustomControllers()
            .AddCustomSwagger()
            .AddCustomAutoMapper()
            .AddCustomMediator()
            .AddCustomDbContext(Configuration)
            .AddCustomOptions(Configuration);

        var iracingOptions = Configuration
            .GetSection(iRacingOptions.Key)
            .Get<iRacingOptions>();

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
        IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (!env.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseCustomSwagger();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCustomSwagger();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}