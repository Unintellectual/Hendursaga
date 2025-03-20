using Hendursaga.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Database configuration
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("POSTGRES_CONNECTION_STRING is not set.");
}

// Add services to the container
builder.Services.AddDbContext<HendursagaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient();  // HTTP Client for API calls
builder.Services.AddLogging();     // Enable logging
builder.Services.AddControllers(); // Enable MVC controllers

// Register JellyfinMetricsService as a background service
builder.Services.AddHostedService<JellyfinMetricsService>();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();


app.MapControllers();

app.Run();
