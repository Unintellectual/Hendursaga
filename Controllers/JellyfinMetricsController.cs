using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hendursaga.Controllers
{
    [ApiController]
    [Route("api/jellyfin")]
    public class JellyfinMetricsController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly ILogger<JellyfinMetricsController> _logger;

        public JellyfinMetricsController(IConfiguration config, ILogger<JellyfinMetricsController> logger)
        {
            _connectionString = config.GetConnectionString("PostgresDB");
            _logger = logger;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var metrics = new List<object>();

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                var query = "SELECT timestamp, active_users, streaming_sessions FROM jellyfin_metrics ORDER BY timestamp DESC LIMIT 10";
                await using var cmd = new NpgsqlCommand(query, conn);
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    metrics.Add(new
                    {
                        Timestamp = reader.GetDateTime(0),
                        ActiveUsers = reader.GetInt32(1),
                        StreamingSessions = reader.GetInt32(2)
                    });
                }

                return Ok(metrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Jellyfin metrics.");
                return StatusCode(500, "Error retrieving metrics");
            }
        }
    }
}
