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
public class EventTypeController : ControllerBase
{
	private readonly IEventTypeService _eventTypeService;
	private readonly IMapper _mapper;

	public EventTypeController(
		IEventTypeService eventTypeService,
		IMapper mapper)
	{
		_eventTypeService = eventTypeService;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<List<EventTypeResponse>>> GetAll(Guid companyId)
	{
		List<EventType> eventTypes = await _eventTypeService.GetAllAsync(companyId);
		List<EventTypeResponse> responses = _mapper.Map<List<EventTypeResponse>>(eventTypes);
		return Ok(responses);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<EventTypeResponse>> GetById(
		Guid id,
		Guid companyId)
	{
		EventType? eventType = await _eventTypeService.GetByIdAsync(id, companyId);
		if (eventType == null)
			return NotFound();

		EventTypeResponse response = _mapper.Map<EventTypeResponse>(eventType);
		return Ok(response);
	}

	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		Guid companyId,
		[FromBody] EventTypeRequest request)
	{
		EventType eventType = _mapper.Map<EventType>(request);
		eventType.SetCompanyId(companyId);
		Guid id = await _eventTypeService.CreateAsync(eventType);
		return CreatedAtAction(nameof(Create), id);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(
		Guid id,
		Guid companyId,
		[FromBody] EventTypeRequest request)
	{
		EventType? eventType = await _eventTypeService.GetByIdAsync(id, companyId);
		if (eventType == null)
			return NotFound();

		_mapper.Map(request, eventType);
		await _eventTypeService.UpdateAsync(eventType);
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(
		Guid id,
		Guid companyId)
	{
		EventType? eventType = await _eventTypeService.GetByIdAsync(id, companyId);
		if (eventType == null)
			return NotFound();

		await _eventTypeService.DeleteAsync(id, companyId);
		return NoContent();
	}
}