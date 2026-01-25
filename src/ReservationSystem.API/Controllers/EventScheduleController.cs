using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Extensions;
using ReservationSystem.Filters;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Contracts.Dtos.Requests;
using ReservationSystem.Contracts.Dtos.Responses;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Controllers;

[ApiController]
[Route("api/[controller]/{companyId:guid}")]
[Authorize(Roles = "Manager")]
[CompanyAccess]
public class EventScheduleController : ControllerBase
{
	private readonly IEventScheduleService _eventScheduleService;
	private readonly IMapper _mapper;

	public EventScheduleController(
		IEventScheduleService eventScheduleService,
		IMapper mapper)
	{
		_eventScheduleService = eventScheduleService;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<PagedResponse<EventScheduleResponse>>> GetAll(
		Guid companyId,
		[FromQuery] EventScheduleFilterRequest filterRequest)
	{
		(List<EventSchedule> Items, int TotalCount) result = await _eventScheduleService
			.GetAllAsync(
				companyId,
				filterRequest.Page,
				filterRequest.PageSize,
				filterRequest.EventTypeId);
		PagedResponse<EventScheduleResponse> response = result
			.ToPagedResponse<EventSchedule, EventScheduleResponse>(filterRequest, _mapper);
		return Ok(response);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<EventScheduleResponse>> GetById(
		Guid id,
		Guid companyId)
	{
		EventSchedule? eventSchedule = await _eventScheduleService.GetByIdAsync(id, companyId);
		if (eventSchedule == null)
			return NotFound();

		EventScheduleResponse response = _mapper.Map<EventScheduleResponse>(eventSchedule);
		return Ok(response);
	}

	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		Guid companyId,
		[FromBody] EventScheduleRequest request)
	{
		EventSchedule eventSchedule = _mapper.Map<EventSchedule>(request);
		eventSchedule.SetCompanyId(companyId);
		Guid id = await _eventScheduleService.CreateAsync(eventSchedule);
		return CreatedAtAction(nameof(Create), id);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(
		Guid id,
		Guid companyId,
		[FromBody] EventScheduleRequest request)
	{
		EventSchedule? eventSchedule = await _eventScheduleService.GetByIdAsync(id, companyId);
		if (eventSchedule == null)
			return NotFound();

		_mapper.Map(request, eventSchedule);
		await _eventScheduleService.UpdateAsync(eventSchedule);
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(
		Guid id,
		Guid companyId)
	{
		EventSchedule? eventSchedule = await _eventScheduleService.GetByIdAsync(id, companyId);
		if (eventSchedule == null)
			return NotFound();

		await _eventScheduleService.DeleteAsync(id, companyId);
		return NoContent();
	}
}