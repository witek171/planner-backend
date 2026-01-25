using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ReservationSystem.Application.Interfaces.Utils;

public interface IHealthCheckUtils
{
	string MaskConnectionString(string connectionString);
	string GetAssemblyVersion(ref string status);
	TimeSpan GetApplicationUptime(ref string status);
	long GetMemoryUsage(ref string status);

	void AddDetailOrLogError(
		Dictionary<string, object> details,
		ref string status,
		string key,
		Func<object> func,
		ILogger logger);

	void HandleDatabaseError(
		Exception ex,
		string message,
		bool isCritical,
		ref string status,
		Dictionary<string, object> details,
		ILogger logger);

	bool IsCriticalSqlError(SqlException sqlEx);
}