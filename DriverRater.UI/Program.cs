using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DriverRater.UI;
using Flurl.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var driverApiOptions = builder.Configuration.GetSection(DriverApiOptions.Key).Get<DriverApiOptions>();

builder.Services.Configure<DriverApiOptions>(builder.Configuration.GetSection(DriverApiOptions.Key));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(driverApiOptions.BaseUrl) });
builder.Services.AddHttpClient<FlurlClient>(client => client.BaseAddress = new Uri("https://localhost:7060"));
builder.Services.AddTransient<IDriverRaterDataService, DriverRaterDataService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
});

await builder.Build().RunAsync();