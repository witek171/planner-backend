using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Services;

public interface IHealthCheckService
{
	ApplicationHealthStatus GetApplicationStatus();
	Task<DatabaseHealthStatus> GetDatabaseStatusAsync();
}