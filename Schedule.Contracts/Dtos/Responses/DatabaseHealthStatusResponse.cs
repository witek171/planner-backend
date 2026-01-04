using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class DatabaseHealthStatusResponse
{
	[Required] public string ConnectionString { get; init; }
	[Required] public TimeSpan ResponseTime { get; init; }
	[Required] public string DatabaseName { get; init; }
	[Required] public string Status { get; init; }
	[Required] public DateTime Timestamp { get; init; }
	[Required] public Dictionary<string, object> Details { get; init; }
}