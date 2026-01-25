using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Filters;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Application.ReadModels;
using ReservationSystem.Contracts.Dtos.Responses;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Controllers;

[ApiController]
[Authorize]
[Route("api/staffMember")]
public class StaffMemberCompanyController : ControllerBase
{
	private readonly IStaffMemberService _staffMemberService;
	private readonly IMapper _mapper;

	public StaffMemberCompanyController(
		IStaffMemberService staffMemberService,
		IMapper mapper)
	{
		_staffMemberService = staffMemberService;
		_mapper = mapper;
	}

	[HttpGet("companies")]
	public async Task<ActionResult<StaffMemberCompaniesResponse>> GetAssignedCompanies()
	{
		string? staffMemberIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (staffMemberIdClaim == null || !Guid.TryParse(staffMemberIdClaim, out Guid currentStaffMemberId))
			return Unauthorized();

		StaffMemberCompanies staffMemberCompanies = await _staffMemberService
			.GetAssignedCompanyAsync(currentStaffMemberId);
		StaffMemberCompaniesResponse response = _mapper.Map<StaffMemberCompaniesResponse>(staffMemberCompanies);
		return Ok(response);
	}

	// dodac sprawdzenie czy staff member nalezy do firmy
	[HttpPost("{companyId:guid}/{staffMemberId:guid}/companies/{targetCompanyId:guid}")]
	[Authorize(Roles = "Manager")]
	[CompanyAccess]
	public async Task<ActionResult> AssignToCompany(
		Guid companyId,
		Guid targetCompanyId,
		Guid staffMemberId)
	{
		// walidacja company(czy mamy ta firme w relacji) i staffmember id
		StaffMember? staffMember = await _staffMemberService.GetByIdAsync(staffMemberId, companyId);
		if (staffMember == null)
			return NotFound();

		Guid id = await _staffMemberService.AssignToCompanyAsync(staffMemberId, targetCompanyId);
		return CreatedAtAction(nameof(AssignToCompany), id);
	}

	[HttpDelete("{companyId:guid}/{staffMemberId:guid}/companies/{targetCompanyId:guid}")]
	[Authorize(Roles = "Manager")]
	[CompanyAccess]
	public async Task<ActionResult> UnassignFromCompany(
		Guid companyId,
		Guid targetCompanyId,
		Guid staffMemberId)
	{
		StaffMember? staffMember = await _staffMemberService.GetByIdAsync(staffMemberId, companyId);
		if (staffMember == null)
			return NotFound();

		await _staffMemberService.UnassignFromCompanyAsync(staffMemberId, targetCompanyId);
		return NoContent();
	}
}