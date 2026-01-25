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
public class ParticipantController : ControllerBase
{
	private readonly IParticipantService _participantService;
	private readonly IMapper _mapper;

	public ParticipantController(
		IParticipantService participantService,
		IMapper mapper)
	{
		_participantService = participantService;
		_mapper = mapper;
	}

	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		Guid companyId,
		[FromBody] ParticipantCreateRequest request)
	{
		Participant participant = _mapper.Map<Participant>(request);
		participant.SetCompanyId(companyId);

		Guid participantId = await _participantService.CreateAsync(participant);
		return CreatedAtAction(nameof(Create), participantId);
	}

	[HttpPut("{participantId:guid}")]
	public async Task<ActionResult> Put(
		Guid companyId,
		Guid participantId,
		[FromBody] ParticipantUpdateRequest request)
	{
		Participant? participant = await _participantService
			.GetByIdAsync(participantId, companyId);
		if (participant == null)
			return NotFound();

		_mapper.Map(request, participant);
		await _participantService.PutAsync(participant);
		return NoContent();
	}

	[HttpDelete("{participantId:guid}")]
	public async Task<ActionResult> DeleteById(
		Guid companyId,
		Guid participantId)
	{
		Participant? participant = await _participantService
			.GetByIdAsync(participantId, companyId);
		if (participant == null)
			return NotFound();

		await _participantService.DeleteByIdAsync(participantId, companyId);
		return NoContent();
	}

	[HttpGet("{participantId:guid}")]
	public async Task<ActionResult<ParticipantResponse>> GetById(
		Guid participantId,
		Guid companyId)
	{
		Participant? participant = await _participantService
			.GetByIdAsync(participantId, companyId);
		ParticipantResponse response = _mapper.Map<ParticipantResponse>(participant);
		return Ok(response);
	}

	[HttpGet]
	public async Task<ActionResult<PagedResponse<ParticipantResponse>>> GetAll(
		Guid companyId,
		[FromQuery] PaginationRequest paginationRequest)
	{
		(List<Participant> Items, int TotalCount) result = await _participantService
			.GetAllAsync(
				companyId,
				paginationRequest.Page,
				paginationRequest.PageSize,
				paginationRequest.Search);
		PagedResponse<ParticipantResponse> response = result
			.ToPagedResponse<Participant, ParticipantResponse>(paginationRequest, _mapper);
		return Ok(response);
	}
}