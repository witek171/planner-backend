using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlannerNet.Filters;

public class StaffMemberAccessAttribute : ActionFilterAttribute
{
	public override async Task OnActionExecutionAsync(
		ActionExecutingContext context,
		ActionExecutionDelegate next)
	{
		if (context.HttpContext.User.IsInRole("Manager"))
		{
			await next();
			return;
		}

		if (!context.ActionArguments.TryGetValue("staffMemberId", out object? staffMemberIdObj) ||
			staffMemberIdObj is not Guid staffMemberId)
		{
			await next();
			return;
		}

		string? userIdClaim = context.HttpContext.User
			.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid currentUserId))
		{
			context.Result = new UnauthorizedResult();
			return;
		}

		if (currentUserId != staffMemberId)
		{
			context.Result = new ForbidResult();
			return;
		}

		await next();
	}
}