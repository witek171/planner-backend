using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlannerNet.Filters;
using Schedule.Application.Interfaces.Services;
using Schedule.Contracts.Dtos.Requests;
using Schedule.Contracts.Dtos.Responses;
using Schedule.Domain.Models;

namespace PlannerNet.Controllers;

[ApiController]
[Route("api/[controller]/{companyId:guid}")]
[Authorize(Roles = "Manager")]
[CompanyAccess]
public class ReservationController : ControllerBase
{
	private readonly IReservationService _reservationService;
	private readonly IMapper _mapper;

	public ReservationController(
		IReservationService reservationService,
		IMapper mapper)
	{
		_reservationService = reservationService;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<List<ReservationResponse>>> GetAll(Guid companyId)
	{
		List<Reservation> reservations = await _reservationService.GetAllAsync(companyId);
		List<ReservationResponse> responses = _mapper.Map<List<ReservationResponse>>(reservations);
		return Ok(responses);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ReservationResponse>> GetById(
		Guid id,
		Guid companyId)
	{
		Reservation? reservation = await _reservationService.GetByIdAsync(id, companyId);
		if (reservation == null)
			return NotFound();

		ReservationResponse response = _mapper.Map<ReservationResponse>(reservation);
		return Ok(response);
	}

	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		Guid companyId,
		[FromBody] ReservationCreateRequest createRequest)
	{
		Reservation reservation = _mapper.Map<Reservation>(createRequest);
		reservation.SetCompanyId(companyId);
		Guid id = await _reservationService.CreateAsync(reservation);
		return CreatedAtAction(nameof(Create), id);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(
		Guid id,
		Guid companyId,
		[FromBody] ReservationUpdateRequest updateRequest)
	{
		Reservation? reservation = await _reservationService.GetByIdAsync(id, companyId);
		if (reservation == null)
			return NotFound();

		_mapper.Map(updateRequest, reservation);
		await _reservationService.UpdateAsync(reservation);
		return NoContent();
	}

	[HttpPatch("{id:guid}/markAsPaid")]
	public async Task<ActionResult> MarkAsPaid(
		Guid id,
		Guid companyId)
	{
		Reservation? reservation = await _reservationService.GetByIdAsync(id, companyId);
		if (reservation == null)
			return NotFound();

		await _reservationService.MarkAsPaidAsync(reservation);
		return NoContent();
	}

	[HttpPatch("{id:guid}/unmarkAsPaid")]
	public async Task<ActionResult> UnmarkAsPaid(
		Guid id,
		Guid companyId)
	{
		Reservation? reservation = await _reservationService.GetByIdAsync(id, companyId);
		if (reservation == null)
			return NotFound();

		await _reservationService.UnmarkAsPaidAsync(reservation);
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(
		Guid id,
		Guid companyId)
	{
		Reservation? reservation = await _reservationService.GetByIdAsync(id, companyId);
		if (reservation == null)
			return NotFound();

		await _reservationService.SoftDeleteAsync(id, companyId);
		return NoContent();
	}
}