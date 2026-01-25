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
public class SpecializationController : ControllerBase
{
	private readonly ISpecializationService _specializationService;
	private readonly IMapper _mapper;

	public SpecializationController(
		ISpecializationService specializationService,
		IMapper mapper)
	{
		_specializationService = specializationService;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<PagedResponse<SpecializationResponse>>> GetAll(
		Guid companyId,
		[FromQuery] PaginationRequest paginationRequest)
	{
		(List<Specialization> Items, int TotalCount) result = await _specializationService
			.GetAllAsync(
				companyId,
				paginationRequest.Page,
				paginationRequest.PageSize,
				paginationRequest.Search);
		PagedResponse<SpecializationResponse> response = result
			.ToPagedResponse<Specialization, SpecializationResponse>(paginationRequest, _mapper);
		return Ok(response);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<SpecializationResponse>> GetById(
		Guid id,
		Guid companyId)
	{
		Specialization? specialization = await _specializationService.GetByIdAsync(id, companyId);
		if (specialization == null)
			return NotFound();

		SpecializationResponse? response = _mapper.Map<SpecializationResponse>(specialization);
		return Ok(response);
	}

	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		Guid companyId,
		[FromBody] SpecializationRequest request)
	{
		Specialization? specialization = _mapper.Map<Specialization>(request);
		specialization.SetCompanyId(companyId);
		Guid id = await _specializationService.CreateAsync(specialization);
		return CreatedAtAction(nameof(Create), id);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(
		Guid id,
		Guid companyId,
		[FromBody] SpecializationRequest request)
	{
		Specialization? specialization = await _specializationService.GetByIdAsync(id, companyId);
		if (specialization == null)
			return NotFound();

		_mapper.Map(request, specialization);
		await _specializationService.UpdateAsync(specialization);
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(
		Guid id,
		Guid companyId)
	{
		Specialization? specialization = await _specializationService.GetByIdAsync(id, companyId);
		if (specialization == null)
			return NotFound();

		await _specializationService.DeleteAsync(id, companyId);
		return NoContent();
	}
}