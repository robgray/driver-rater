namespace DriverRater;

using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
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
                    .WriteTo.Console());
}