namespace ReservationSystem.Domain.Models;

public class DatabaseHealthStatus
{
	public DatabaseHealthStatus(
		string connectionString,
		TimeSpan responseTime,
		string databaseName,
		string status,
		DateTime timestamp,
		Dictionary<string, object> details)
	{
		ConnectionString = connectionString;
		ResponseTime = responseTime;
		DatabaseName = databaseName;
		Status = status;
		Timestamp = timestamp;
		Details = details;
	}

	public string ConnectionString { get; }
	public TimeSpan ResponseTime { get; }
	public string DatabaseName { get; }
	public string Status { get; }
	public DateTime Timestamp { get; }
	public Dictionary<string, object> Details { get; }
}