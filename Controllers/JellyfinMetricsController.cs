using Microsoft.AspNetCore.Mvc;
using Hendursaga.Services;
using Microsoft.EntityFrameworkCore;

namespace Hendursaga.Controllers
{
    [ApiController]
    [Route("api/jellyfin")]
    public class JellyfinMetricsController : ControllerBase
    {
        private readonly HendursagaDbContext _dbContext;
        private readonly ILogger<JellyfinMetricsController> _logger;

        public JellyfinMetricsController(HendursagaDbContext dbContext, ILogger<JellyfinMetricsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            try
            {
                var metrics = await _dbContext.JellyfinMetrics
                    .OrderByDescending(m => m.Timestamp)
                    .Take(10)
                    .ToListAsync();

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
