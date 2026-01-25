using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Filters;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Contracts.Dtos.Requests;
using ReservationSystem.Contracts.Dtos.Responses;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]
public class CompanyController : ControllerBase
{
	private readonly ICompanyService _companyService;
	private readonly ICompanyConfigService _companyConfigService;
	private readonly IMapper _mapper;

	public CompanyController(
		ICompanyService companyService,
		ICompanyConfigService companyConfigService,
		IMapper mapper)
	{
		_companyService = companyService;
		_companyConfigService = companyConfigService;
		_mapper = mapper;
	}

	[HttpPost]
	public async Task<ActionResult<Guid>> Create([FromBody] CompanyRequest request)
	{
		Company company = _mapper.Map<Company>(request);
		Guid companyId = await _companyService.CreateAsync(company);
		return CreatedAtAction(nameof(Create), companyId);
	}

	[HttpPut("{companyId:guid}")]
	[CompanyAccess]
	public async Task<ActionResult> Put(
		Guid companyId,
		[FromBody] CompanyRequest request)
	{
		Company? company = await _companyService.GetByIdAsync(companyId);
		if (company == null)
			return NotFound();

		_mapper.Map(request, company);
		await _companyService.PutAsync(company);
		return NoContent();
	}

	[HttpDelete("{companyId:guid}")]
	[CompanyAccess]
	public async Task<ActionResult> DeleteById(Guid companyId)
	{
		Company? company = await _companyService.GetByIdAsync(companyId);
		if (company == null)
			return NotFound();

		await _companyService.DeleteByIdAsync(companyId);
		return NoContent();
	}

	[HttpGet("{companyId:guid}")]
	public async Task<ActionResult<CompanyResponse>> GetById(Guid companyId)
	{
		Company? company = await _companyService.GetByIdAsync(companyId);
		if (company == null)
			return NotFound();

		CompanyResponse response = _mapper.Map<CompanyResponse>(company);
		return Ok(response);
	}

	[HttpPatch("{companyId:guid}/markAsReception")]
	[CompanyAccess]
	public async Task<ActionResult> MarkAsReception(Guid companyId)
	{
		Company? company = await _companyService.GetByIdAsync(companyId);
		if (company == null)
			return NotFound();

		await _companyService.MarkAsReceptionAsync(company);
		return NoContent();
	}

	[HttpPatch("{companyId:guid}/unmarkAsReception")]
	[CompanyAccess]
	public async Task<ActionResult> UnmarkAsReception(Guid companyId)
	{
		Company? company = await _companyService.GetByIdAsync(companyId);
		if (company == null)
			return NotFound();

		await _companyService.UnmarkAsReceptionAsync(company);
		return NoContent();
	}

	[HttpPost("{companyId:guid}/relation")]
	[CompanyAccess]
	public async Task<ActionResult> AddToParent(
		Guid companyId,
		[FromBody] Guid parentCompanyId)
	{
		Company? company = await _companyService.GetByIdAsync(companyId);
		Company? parentCompany = await _companyService.GetByIdAsync(parentCompanyId);
		if (company == null || parentCompany == null)
			return NotFound();

		await _companyService.AddRelationAsync(companyId, parentCompanyId);
		return Ok();
	}

	[HttpDelete("{companyId:guid}/relation")]
	[CompanyAccess]
	public async Task<ActionResult> RemoveRelations(Guid companyId)
	{
		Company? company = await _companyService.GetByIdAsync(companyId);
		if (company == null)
			return NotFound();

		await _companyService.RemoveRelationsAsync(companyId);
		return NoContent();
	}

	[HttpGet("{companyId:guid}/relation")]
	[CompanyAccess]
	public async Task<ActionResult> GetRelations(Guid companyId)
	{
		Company? company = await _companyService.GetByIdAsync(companyId);
		if (company == null)
			return NotFound();

		List<Company> companies = await _companyService.GetAllRelationsAsync(companyId);
		List<CompanyResponse> responses = _mapper.Map<List<CompanyResponse>>(companies);
		return Ok(responses);
	}

	[HttpPut("{companyId:guid}/breakTimes")]
	[CompanyAccess]
	public async Task<ActionResult> UpdateCompanyBreakTimes(
		Guid companyId,
		[FromBody] UpdateCompanyBreakTimesRequest request)
	{
		CompanyConfig? companyConfig = await _companyConfigService.GetByIdAsync(companyId);
		if (companyConfig == null)
			return NotFound();

		_mapper.Map(request, companyConfig);
		await _companyConfigService.UpdateBreakTimesAsync(companyConfig);
		return NoContent();
	}

	[HttpGet("{companyId:guid}/breakTimes")]
	[CompanyAccess]
	public async Task<ActionResult<CompanyConfigResponse>> GetCompanyBreakTimes(Guid companyId)
	{
		CompanyConfig? companyConfig = await _companyConfigService.GetByIdAsync(companyId);
		if (companyConfig == null)
			return NotFound();

		CompanyConfigResponse response = _mapper.Map<CompanyConfigResponse>(companyConfig);
		return Ok(response);
	}
}