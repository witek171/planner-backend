using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Contracts.Dtos.Requests;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly IMapper _mapper;

	public AuthController(
		IAuthService authService,
		IMapper mapper)
	{
		_authService = authService;
		_mapper = mapper;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(
		[FromBody] LoginRequest request)
	{
		String token = await _authService.LoginAsync(request.Email, request.Password);
		return Ok(new { token });
	}

	[HttpPost("register")]
	public async Task<ActionResult<Guid>> Register(
		[FromBody] StaffMemberRequest request)
	{
		StaffMember staffMember = _mapper.Map<StaffMember>(request);
		Guid staffMemberId = await _authService.RegisterAsync(staffMember);
		return CreatedAtAction(nameof(Register), staffMemberId);
	}
}