using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class ApplicationHealthStatusResponse
{
	[Required] public string Version { get; init; }
	[Required] public string Environment { get; init; }
	[Required] public TimeSpan Uptime { get; init; }
	[Required] public long MemoryUsage { get; init; }
	[Required] public string Status { get; init; }
	[Required] public DateTime Timestamp { get; init; }
	[Required] public Dictionary<string, object> Details { get; init; }
}