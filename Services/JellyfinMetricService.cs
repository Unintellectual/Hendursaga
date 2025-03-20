using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Hendursaga.Services;
using Hendursaga.Models;

namespace Hendursaga.Services
{
    public class JellyfinMetricsService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JellyfinMetricsService> _logger;
        private readonly HendursagaDbContext _dbContext;
        private readonly string _jellyfinUrl;
        private readonly string _apiKey;

        public JellyfinMetricsService(HttpClient httpClient, ILogger<JellyfinMetricsService> logger, HendursagaDbContext dbContext)
        {
            _httpClient = httpClient;
            _logger = logger;
            _dbContext = dbContext;

            _jellyfinUrl = Environment.GetEnvironmentVariable("JELLYFIN_URL")?.Trim();
            _apiKey = Environment.GetEnvironmentVariable("JELLYFIN_API_KEY")?.Trim();

            if (string.IsNullOrEmpty(_jellyfinUrl) || string.IsNullOrEmpty(_apiKey))
            {
                _logger.LogError("JELLYFIN_URL or JELLYFIN_API_KEY is not set. Please check your environment variables.");
                throw new InvalidOperationException("Jellyfin URL and API Key must be set.");
            }

            _logger.LogInformation("JellyfinMetricsService initialized with URL: {JellyfinUrl}", _jellyfinUrl);
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
                var requestUrl = $"{_jellyfinUrl}/Sessions?api_key={_apiKey}";
                _logger.LogDebug("Fetching Jellyfin metrics from {RequestUrl}", requestUrl);

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

                // Store in SQLite using EF Core
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
            var metric = new JellyfinMetric
            {
                ActiveUsers = activeUsers,
                StreamingSessions = sessionCount
            };

            _dbContext.JellyfinMetrics.Add(metric);
            await _dbContext.SaveChangesAsync();
        }
    }
}
