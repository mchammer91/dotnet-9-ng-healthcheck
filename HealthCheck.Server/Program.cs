global using HealthCheck.Server;
global using HealthCheck.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks()
    .AddCheck<ICMPHealthCheck>("IMCP");

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Above is initializing the services and features to be used by the web host (called container above)
// Below initializes the application and sets up the request / response pipeline

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware is registered top-to-bottom
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHealthChecks(new PathString("/api/health"));

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
