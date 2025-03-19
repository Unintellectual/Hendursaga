using Prometheus;
using Hendursaga.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();  // HTTP Client for API calls
builder.Services.AddLogging();     // Enable logging

// Register JellyfinMetricsService as a background service
builder.Services.AddSingleton<JellyfinMetricsService>();
builder.Services.AddHostedService<JellyfinMetricsService>();

var app = builder.Build();

// Enable Prometheus metrics with top-level route registration
app.MapMetrics(); // `/metrics` will be exposed

app.Run();