namespace ReservationSystem.Domain.Models;

public class ApplicationHealthStatus
{
	public ApplicationHealthStatus(
		string version,
		string environment,
		TimeSpan uptime,
		long memoryUsage,
		string status,
		DateTime timestamp,
		Dictionary<string, object> details)
	{
		Version = version;
		Environment = environment;
		Uptime = uptime;
		MemoryUsage = memoryUsage;
		Status = status;
		Timestamp = timestamp;
		Details = details;
	}

	public string Version { get; }
	public string Environment { get; }
	public TimeSpan Uptime { get; }
	public long MemoryUsage { get; }
	public string Status { get; }
	public DateTime Timestamp { get; }
	public Dictionary<string, object> Details { get; }
}