namespace DriverRater.Api;

using System.Reflection;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")
            .CreateBootstrapLogger(); // <- this means it is temp & gets reconfigured/replaced by the host later on :-)

        var host = CreateHostBuilder(args).Build();
        
        await host.RunAsync();
    }
    
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(opt => opt.AddServerHeader = false)
                        .ConfigureAppConfiguration(builder => builder.AddUserSecrets<Program>())
                        .UseStartup<Startup>();
                })
            .ConfigureLogging(logging => logging.ClearProviders())
            .UseSerilog(
                (context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) // because we are using app.UseSerilogRequestLogging() in Startup.cs instead
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails(
                        new DestructuringOptionsBuilder()
                            .WithDefaultDestructurers()
                            .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() }))
                    .Enrich.WithMachineName()
                    .WriteTo.Console());
}