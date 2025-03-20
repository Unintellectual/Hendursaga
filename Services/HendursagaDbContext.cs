using Microsoft.EntityFrameworkCore;
using Hendursaga.Models;

namespace Hendursaga.Services;

public class HendursagaDbContext : DbContext
{
    public HendursagaDbContext(DbContextOptions<HendursagaDbContext> options) : base(options) { }

    public DbSet<JellyfinMetric> JellyfinMetrics { get; set; }
}
