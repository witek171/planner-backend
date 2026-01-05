using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Schedule.Application.Interfaces.Services;
using System.Security.Claims;
using Schedule.Application.ReadModels;
using Schedule.Domain.Models;

namespace PlannerNet.Filters;

public class CompanyAccessAttribute : ActionFilterAttribute
{
	public override async Task OnActionExecutionAsync(
		ActionExecutingContext context,
		ActionExecutionDelegate next)
	{
		IStaffMemberService staffMemberService = context.HttpContext.RequestServices
			.GetRequiredService<IStaffMemberService>();

		if (!context.ActionArguments.TryGetValue("companyId", out object? companyIdObj) ||
			companyIdObj is not Guid companyId)
		{
			context.Result = new BadRequestResult();
			return;
		}

		string? userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid currentUserId))
		{
			context.Result = new UnauthorizedResult();
			return;
		}

		StaffMemberCompanies userCompanies = await staffMemberService
			.GetAssignedCompanyAsync(currentUserId);
		if (!userCompanies.Companies.Any(sc => sc.Id == companyId))
		{
			context.Result = new ForbidResult();
			return;
		}

		await next();
	}
}