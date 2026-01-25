using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Contracts.Dtos.Responses;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class HealthCheckController : ControllerBase
{
	private readonly IHealthCheckService _healthCheckService;
	private readonly IMapper _mapper;

	public HealthCheckController(
		IHealthCheckService healthCheckService,
		IMapper mapper)
	{
		_healthCheckService = healthCheckService;
		_mapper = mapper;
	}

	[HttpGet("application")]
	public ActionResult<ApplicationHealthStatusResponse> GetApplicationStatus()
	{
		ApplicationHealthStatus health = _healthCheckService.GetApplicationStatus();

		ApplicationHealthStatusResponse response = _mapper.Map<ApplicationHealthStatusResponse>(health);

		return health.Status switch
		{
			"Healthy" or "Degraded" => Ok(response),
			"Unhealthy" => StatusCode(503, response)
		};
	}

	[HttpGet("database")]
	public async Task<ActionResult<DatabaseHealthStatusResponse>> GetDatabaseStatusAsync()
	{
		DatabaseHealthStatus health = await _healthCheckService.GetDatabaseStatusAsync();

		DatabaseHealthStatusResponse response = _mapper.Map<DatabaseHealthStatusResponse>(health);

		return health.Status switch
		{
			"Healthy" or "Degraded" => Ok(response),
			"Unhealthy" => StatusCode(503, response)
		};
	}
}