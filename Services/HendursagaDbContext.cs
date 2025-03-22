using Microsoft.EntityFrameworkCore;
using Hendursaga.Models;

namespace Hendursaga.Services
{
    public class HendursagaDbContext : DbContext
    {
        public DbSet<JellyfinMetric> JellyfinMetrics { get; set; }

        public HendursagaDbContext(DbContextOptions<HendursagaDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=hendursaga.db");
            }
        }
    }
}
