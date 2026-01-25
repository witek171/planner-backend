using System.Diagnostics;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Application.Interfaces.Utils;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Infrastructure.Services;

public class HealthCheckService : IHealthCheckService
{
	private readonly IHealthCheckUtils _healthCheckUtils;
	private readonly ILogger<HealthCheckService> _logger;
	private readonly string _connectionString;

	public HealthCheckService(
		IHealthCheckUtils healthCheckUtils,
		ILogger<HealthCheckService> logger,
		string connectionString)
	{
		_healthCheckUtils = healthCheckUtils;
		_logger = logger;
		_connectionString = connectionString;
	}

	public ApplicationHealthStatus GetApplicationStatus()
	{
		_logger.LogInformation("Checking application health");

		string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown";
		string status = "Healthy";
		Dictionary<string, object> details = new();
		string version = _healthCheckUtils.GetAssemblyVersion(ref status);
		TimeSpan uptime = _healthCheckUtils.GetApplicationUptime(ref status);
		long memoryUsage = _healthCheckUtils.GetMemoryUsage(ref status);

		_healthCheckUtils.AddDetailOrLogError(details, ref status, "machineName",
			() => Environment.MachineName, _logger);
		_healthCheckUtils.AddDetailOrLogError(details, ref status, "processorCount",
			() => Environment.ProcessorCount, _logger);
		_healthCheckUtils.AddDetailOrLogError(details, ref status, "osVersion",
			() => Environment.OSVersion.ToString(), _logger);
		_healthCheckUtils.AddDetailOrLogError(details, ref status, "workingDirectory",
			() => Environment.CurrentDirectory, _logger);
		_healthCheckUtils.AddDetailOrLogError(details, ref status, "assemblyLocation",
			() => Assembly.GetExecutingAssembly().Location ?? "Unknown", _logger);
		_healthCheckUtils.AddDetailOrLogError(details, ref status, "dotnetVersion",
			() => Environment.Version.ToString(), _logger);
		_healthCheckUtils.AddDetailOrLogError(details, ref status, "is64BitProcess",
			() => Environment.Is64BitProcess, _logger);
		_healthCheckUtils.AddDetailOrLogError(details, ref status, "totalMemory", 
			() => GC.GetTotalMemory(false), _logger);

		if (status == "Degraded")
			details["dataQuality"] = "Some metrics unavailable";

		return new ApplicationHealthStatus(
			version,
			environment,
			uptime,
			memoryUsage,
			status,
			DateTime.UtcNow,
			details);
	}

	public async Task<DatabaseHealthStatus> GetDatabaseStatusAsync()
	{
		_logger.LogInformation("Checking database health");

		Stopwatch stopwatch = Stopwatch.StartNew();
		string databaseName = "Unknown";
		string status = "Healthy";
		Dictionary<string, object> details = new();
		string connectionString = _connectionString;
		string maskedConnectionString = _healthCheckUtils.MaskConnectionString(connectionString);

		try
		{
			const int timeoutSeconds = 10;
			TimeSpan databaseTimeout = TimeSpan.FromSeconds(timeoutSeconds);
			using CancellationTokenSource timeoutCts = new(databaseTimeout);

			await using SqlConnection connection = new(connectionString);
			await connection.OpenAsync(timeoutCts.Token);

			await using SqlCommand testCommand = new("SELECT 1", connection);
			await testCommand.ExecuteScalarAsync(timeoutCts.Token);

			databaseName = connection.Database ?? "Unknown";
			string serverVersion = connection.ServerVersion ?? "Unknown";

			details["serverVersion"] = serverVersion;
			details["connectionTimeout"] = connection.ConnectionTimeout;
			details["state"] = connection.State.ToString();
			details["serverProcessId"] = connection.ServerProcessId.ToString();
			details["clientConnectionId"] = connection.ClientConnectionId.ToString();
		}
		catch (OperationCanceledException opEx)
		{
			_healthCheckUtils.HandleDatabaseError(opEx, "Database connection timeout", true,
				ref status, details, _logger);
		}
		catch (SqlException sqlEx)
		{
			_healthCheckUtils.HandleDatabaseError(sqlEx, sqlEx.Message, _healthCheckUtils.IsCriticalSqlError(sqlEx),
				ref status, details, _logger);

			details["sqlErrorNumber"] = sqlEx.Number;
			details["severity"] = sqlEx.Class;
			details["state"] = sqlEx.State;
		}
		catch (Exception ex)
		{
			_healthCheckUtils.HandleDatabaseError(ex, ex.Message, true, ref status, details, _logger);
		}
		finally
		{
			stopwatch.Stop();
		}

		if (databaseName == "Unknown" || stopwatch.Elapsed > TimeSpan.FromSeconds(5))
		{
			status = status == "Healthy" ? "Degraded" : status;
			details["dataQuality"] = "Performance or metadata issues";
		}

		return new DatabaseHealthStatus(
			maskedConnectionString,
			stopwatch.Elapsed,
			databaseName,
			status,
			DateTime.UtcNow,
			details);
	}
}