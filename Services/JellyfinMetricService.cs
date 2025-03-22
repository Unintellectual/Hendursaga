using Hendursaga.Models;


namespace Hendursaga.Services
{
    public class JellyfinMetricsService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JellyfinMetricsService> _logger;
        private readonly IServiceScopeFactory _scopeFactory; // Use scope factory to get DbContext

        public JellyfinMetricsService(HttpClient httpClient, ILogger<JellyfinMetricsService> logger, IServiceScopeFactory scopeFactory)
        {
            _httpClient = httpClient;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CollectMetricsAsync();
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        private async Task CollectMetricsAsync()
        {
            try
            {
                var requestUrl = $"{Environment.GetEnvironmentVariable("JELLYFIN_URL")}/Sessions?api_key={Environment.GetEnvironmentVariable("JELLYFIN_API_KEY")}";
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch Jellyfin metrics: {StatusCode}", response.StatusCode);
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();
                var sessions = JsonSerializer.Deserialize<JsonElement>(content);

                if (sessions.ValueKind != JsonValueKind.Array)
                {
                    _logger.LogWarning("Unexpected response format from Jellyfin API.");
                    return;
                }

                int activeUsers = sessions.EnumerateArray()
                    .Select(s => s.GetProperty("UserId").GetString())
                    .Distinct()
                    .Count();

                int sessionCount = sessions.GetArrayLength();

                // Store in SQLite using a scoped DbContext
                await StoreMetricsAsync(activeUsers, sessionCount);

                _logger.LogInformation("Jellyfin Metrics Updated: Users={Users}, Sessions={Sessions}", activeUsers, sessionCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Jellyfin metrics.");
            }
        }

        private async Task StoreMetricsAsync(int activeUsers, int sessionCount)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HendursagaDbContext>();

            var metric = new JellyfinMetric
            {
                ActiveUsers = activeUsers,
                StreamingSessions = sessionCount,
                Timestamp = DateTime.UtcNow
            };

            dbContext.JellyfinMetrics.Add(metric);
            await dbContext.SaveChangesAsync();
        }
    }
}
