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
[CompanyAccess]
public class StaffMemberController : ControllerBase
{
	private readonly IStaffMemberService _staffMemberService;
	private readonly IStaffMemberSpecializationService _staffMemberSpecializationService;
	private readonly IStaffMemberAvailabilityService _staffMemberAvailabilityService;
	private readonly IEventScheduleStaffMemberService _eventScheduleStaffMemberService;
	private readonly IEventScheduleService _eventScheduleService;
	private readonly IMapper _mapper;

	public StaffMemberController(
		IStaffMemberService staffMemberService,
		IStaffMemberSpecializationService staffMemberSpecializationService,
		IStaffMemberAvailabilityService staffMemberAvailabilityService,
		IEventScheduleStaffMemberService eventScheduleStaffMemberService,
		IEventScheduleService eventScheduleService,
		IMapper mapper)
	{
		_staffMemberService = staffMemberService;
		_staffMemberSpecializationService = staffMemberSpecializationService;
		_staffMemberAvailabilityService = staffMemberAvailabilityService;
		_eventScheduleStaffMemberService = eventScheduleStaffMemberService;
		_eventScheduleService = eventScheduleService;
		_mapper = mapper;
	}

	[HttpGet]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<List<StaffMemberResponse>>> GetAll(
		Guid companyId,
		[FromQuery] PaginationRequest paginationRequest)
	{
		(List<StaffMember> Items, int TotalCount) result = await _staffMemberService
			.GetAllAsync(companyId, paginationRequest.Page, paginationRequest.PageSize);
		PagedResponse<StaffMemberResponse> response = result
			.ToPagedResponse<StaffMember, StaffMemberResponse>(paginationRequest, _mapper);
		return Ok(response);
	}

	[HttpGet("{staffMemberId:guid}")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<StaffMemberResponse>> GetById(
		Guid staffMemberId,
		Guid companyId)
	{
		StaffMember? staffMember = await _staffMemberService
			.GetByIdAsync(staffMemberId, companyId);
		if (staffMember == null)
			return NotFound();

		StaffMemberResponse response = _mapper.Map<StaffMemberResponse>(staffMember);
		return Ok(response);
	}

	[HttpPost]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<Guid>> Create(
		Guid companyId,
		[FromBody] StaffMemberRequest request)
	{
		StaffMember staffMember = _mapper.Map<StaffMember>(request);
		Guid staffMemberId = await _staffMemberService.CreateAsync(staffMember, companyId);
		return CreatedAtAction(nameof(Create), staffMemberId);
	}

	[HttpPut("{staffMemberId:guid}")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult> Put(
		Guid staffMemberId,
		Guid companyId,
		[FromBody] StaffMemberRequest request)
	{
		StaffMember? staffMember = await _staffMemberService
			.GetByIdAsync(staffMemberId, companyId);
		if (staffMember == null)
			return NotFound();

		_mapper.Map(request, staffMember);
		await _staffMemberService.PutAsync(staffMember, companyId);
		return NoContent();
	}

	[HttpDelete("{staffMemberId:guid}")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult> Delete(
		Guid staffMemberId,
		Guid companyId)
	{
		StaffMember? staffMember = await _staffMemberService
			.GetByIdAsync(staffMemberId, companyId);
		if (staffMember == null)
			return NotFound();

		await _staffMemberService.DeleteAsync(staffMemberId, companyId);
		return NoContent();
	}

	[HttpPost("specialization")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<Guid>> CreateStaffMemberSpecialization(
		Guid companyId,
		[FromBody] StaffMemberSpecializationRequest request)
	{
		StaffMemberSpecialization staffMemberSpecialization = _mapper
			.Map<StaffMemberSpecialization>(request);
		staffMemberSpecialization.SetCompanyId(companyId);
		Guid id = await _staffMemberSpecializationService
			.CreateAsync(companyId, staffMemberSpecialization);
		return CreatedAtAction(nameof(CreateStaffMemberSpecialization), id);
	}

	[HttpDelete("specialization/{staffMemberSpecializationId:guid}")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult> DeleteStaffMemberSpecialization(
		Guid companyId,
		Guid staffMemberSpecializationId)
	{
		bool exists = await _staffMemberSpecializationService
			.ExistsByIdAsync(companyId, staffMemberSpecializationId);
		if (!exists)
			return NotFound();

		await _staffMemberSpecializationService
			.DeleteAsync(companyId, staffMemberSpecializationId);
		return NoContent();
	}

	[HttpGet("availability/{staffMemberId:guid}")]
	[Authorize(Roles = "Manager,Trainer")]
	[StaffMemberAccess]
	public async Task<ActionResult<List<StaffMemberAvailabilityResponse>>> GetAvailabilityByStaffMemberId(
		Guid companyId,
		Guid staffMemberId)
	{
		StaffMember? staffMember = await _staffMemberService
			.GetByIdAsync(staffMemberId, companyId);
		if (staffMember == null)
			return NotFound();

		List<StaffMemberAvailability> availabilities =
			await _staffMemberAvailabilityService
				.GetByStaffMemberIdAsync(companyId, staffMemberId);
		StaffMemberAvailabilityResponse response = new(
			_mapper.Map<StaffMemberResponse>(staffMember),
			_mapper.Map<List<AvailableSlotResponse>>(availabilities));
		return Ok(response);
	}

	[HttpPost("availability/{staffMemberId:guid}")]
	[Authorize(Roles = "Manager,Trainer")]
	[StaffMemberAccess]
	public async Task<ActionResult<Guid>> CreateAvailability(
		Guid companyId,
		Guid staffMemberId,
		[FromBody] StaffMemberAvailabilityRequest request)
	{
		StaffMember? staffMember = await _staffMemberService
			.GetByIdAsync(staffMemberId, companyId);
		if (staffMember == null)
			return NotFound();

		StaffMemberAvailability availability = _mapper.Map<StaffMemberAvailability>(request);
		availability.SetCompanyId(companyId);
		availability.SetStaffMemberId(staffMemberId);
		Guid id = await _staffMemberAvailabilityService.CreateAsync(availability);
		return CreatedAtAction(nameof(CreateAvailability), id);
	}

	[HttpDelete("availability/{availabilityId:guid}")]
	[Authorize(Roles = "Manager,Trainer")]
	[StaffMemberAccess]
	public async Task<ActionResult> DeleteAvailability(
		Guid companyId,
		Guid availabilityId)
	{
		bool exists = await _staffMemberAvailabilityService
			.ExistsByIdAsync(companyId, availabilityId);
		if (!exists)
			return NotFound();

		await _staffMemberAvailabilityService.DeleteAsync(companyId, availabilityId);
		return NoContent();
	}

	[HttpGet("eventSchedules/{staffMemberId:guid}")]
	[Authorize(Roles = "Manager,Trainer")]
	[StaffMemberAccess]
	public async Task<ActionResult<List<EventScheduleResponse>>> GetStaffMemberEventSchedules(
		Guid companyId,
		Guid staffMemberId)
	{
		List<EventSchedule> schedules = await _eventScheduleService
			.GetByStaffMemberIdAsync(companyId, staffMemberId);
		List<EventScheduleResponse> responses = _mapper
			.Map<List<EventScheduleResponse>>(schedules);
		return Ok(responses);
	}

	[HttpPost("eventSchedule")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult<Guid>> AssignStaffMemberToEvent(
		Guid companyId,
		[FromBody] EventScheduleStaffMemberRequest request)
	{
		EventScheduleStaffMember eventScheduleStaffMember = _mapper
			.Map<EventScheduleStaffMember>(request);
		eventScheduleStaffMember.SetCompanyId(companyId);
		Guid id = await _eventScheduleStaffMemberService
			.CreateAsync(eventScheduleStaffMember);
		return Ok(id);
	}

	[HttpDelete("eventSchedule/{eventScheduleStaffMemberId:guid}")]
	[Authorize(Roles = "Manager")]
	public async Task<ActionResult> UnassignStaffMemberFromEvent(
		Guid companyId,
		Guid eventScheduleStaffMemberId)
	{
		bool exists = await _eventScheduleStaffMemberService
			.ExistsByIdAsync(companyId, eventScheduleStaffMemberId);
		if (!exists)
			return NotFound();

		await _eventScheduleStaffMemberService.DeleteAsync(companyId, eventScheduleStaffMemberId);
		return NoContent();
	}
}