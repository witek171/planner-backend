using System.Diagnostics;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ReservationSystem.Application.Interfaces.Utils;

namespace ReservationSystem.Infrastructure.Utils;

public class HealthCheckUtils : IHealthCheckUtils
{
	private readonly ILogger<HealthCheckUtils> _logger;

	public HealthCheckUtils(ILogger<HealthCheckUtils> logger)
	{
		_logger = logger;
	}

	public string MaskConnectionString(string connectionString)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
			return "Not configured";

		try
		{
			SqlConnectionStringBuilder builder = new(connectionString);

			if (!string.IsNullOrEmpty(builder.Password))
				builder.Password = "***";

			if (!string.IsNullOrEmpty(builder.UserID))
				builder.UserID = "***";

			return builder.ConnectionString;
		}
		catch (Exception ex)
		{
			string message = ex is ArgumentException
				? "Invalid connection string format during masking"
				: "Cannot mask connection string";

			_logger.LogWarning(ex, message);
			return "Invalid connection string format";
		}
	}

	public string GetAssemblyVersion(ref string status)
	{
		try
		{
			Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			return assembly.GetName().Version?.ToString() ?? "Unknown";
		}
		catch (Exception ex)
		{
			if (IsSystemCriticalError(ex))
			{
				status = "Unhealthy";
				_logger.LogCritical(ex, "Critical error while retrieving assembly version");
			}
			else if (status != "Unhealthy")
			{
				status = "Degraded";
				_logger.LogWarning(ex, "Error while retrieving assembly version");
			}

			return "Unknown";
		}
	}

	public TimeSpan GetApplicationUptime(ref string status)
	{
		try
		{
			using Process process = Process.GetCurrentProcess();
			return DateTime.UtcNow - process.StartTime.ToUniversalTime();
		}
		catch (Exception ex)
		{
			if (IsSystemCriticalError(ex))
			{
				status = "Unhealthy";
				_logger.LogCritical(ex, "Critical error while calculating uptime");
			}
			else if (status != "Unhealthy")
			{
				status = "Degraded";
				_logger.LogWarning(ex, "Unable to calculate application uptime");
			}

			return TimeSpan.Zero;
		}
	}

	public long GetMemoryUsage(ref string status)
	{
		try
		{
			using Process process = Process.GetCurrentProcess();
			return process.WorkingSet64;
		}
		catch (Exception ex)
		{
			if (IsSystemCriticalError(ex))
			{
				status = "Unhealthy";
				_logger.LogCritical(ex, "Critical error while retrieving memory usage");
			}
			else if (status != "Unhealthy")
			{
				status = "Degraded";
				_logger.LogWarning(ex, "Unable to get memory usage");
			}

			return 0;
		}
	}

	public void AddDetailOrLogError(
		Dictionary<string, object> details,
		ref string status,
		string key,
		Func<object> func,
		ILogger logger)
	{
		try
		{
			details[key] = func() ?? "Unknown";
		}
		catch (Exception ex)
		{
			details[$"{key}_error"] = ex.Message;

			if (IsSystemCriticalError(ex))
			{
				status = "Unhealthy";
				logger.LogCritical(ex, $"Critical error while retrieving '{key}'");
			}
			else if (status != "Unhealthy")
			{
				status = "Degraded";
				logger.LogWarning(ex, $"Non-critical error while retrieving '{key}'");
			}
		}
	}

	public void HandleDatabaseError(
		Exception ex,
		string message,
		bool isCritical,
		ref string status,
		Dictionary<string, object> details,
		ILogger logger)
	{
		logger.LogError(ex, "Database health check failed: {Message}", message);

		status = isCritical ? "Unhealthy" : "Degraded";
		details["error"] = message;
		details["errorType"] = ex.GetType().Name;
	}

	public bool IsCriticalSqlError(SqlException sqlEx)
	{
		int[] criticalErrors = new[]
		{
			18456, 4060, 18452, 233, -2, 53, 2, 11001
		};

		return criticalErrors.Contains(sqlEx.Number) || sqlEx.Class >= 20;
	}

	private bool IsSystemCriticalError(Exception ex)
	{
		return ex
			is BadImageFormatException
			or InvalidProgramException;
	}
}