namespace Hendursaga.Models;

public class JellyfinMetric
{
    public int Id { get; set; }
    public int ActiveUsers { get; set; }
    public int StreamingSessions { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
