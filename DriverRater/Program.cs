using HelmetRanker.Plumbing.Automapper;
using HelmetRanker.Plumbing.Controllers;
using HelmetRanker.Plumbing.Mediator;
using HelmetRanker.Plumbing.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomControllers();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomAutoMapper();
builder.Services.AddCustomMediator();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseCustomSwagger();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();