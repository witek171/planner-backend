using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Schedule.Domain.Exceptions;

namespace Schedule.Infrastructure.Extensions;

public class GlobalExceptionMiddleware
{
	private readonly RequestDelegate _requestDelegate;
	private readonly ILogger<GlobalExceptionMiddleware> _logger;
	private readonly IHostEnvironment _environment;

	public GlobalExceptionMiddleware(
		RequestDelegate requestDelegate,
		ILogger<GlobalExceptionMiddleware> logger,
		IHostEnvironment environment)
	{
		_requestDelegate = requestDelegate;
		_logger = logger;
		_environment = environment;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _requestDelegate(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception occurred. Request: {Method} {Path}",
				context.Request.Method, context.Request.Path);

			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(
		HttpContext context,
		Exception exception)
	{
		if (context.Response.HasStarted)
		{
			_logger.LogWarning("Response has already started, cannot write error response");
			return;
		}

		context.Response.ContentType = "application/json";
		(int statusCode, string userMessage) = exception switch
		{
			CompanySelfReferenceException
				=> (400, "Company cannot be assigned to itself"),
			DuplicateParticipantsException
				=> (400, "List contains duplicate participants"),
			GdprConsentRequiredException
				=> (400, "GDPR consent is required"),
			InvalidBreakTimeParticipantsException
				=> (400, "Invalid break time for participants"),
			InvalidBreakTimeStaffException
				=> (400, "Invalid break time for staff member"),
			InvalidCredentialsException
				=> (401, "Invalid email or password"),
			EventScheduleNotFoundException
				=> (404, "Event schedule not found"),
			EventTypeNotFoundException
				=> (404, "Event type not found"),
			ParticipantNotFoundException
				=> (404, "Participant not found"),
			StaffMemberNotFoundException
				=> (404, "Staff member not found"),
			CompanyNotInHierarchyException
				=> (404, "Company is not present in the hierarchy, therefore it has no relations to remove"),
			CompanyAlreadyHasParentException
				=> (409, "Company is already assigned to another parent company"),
			CompanyRelationAlreadyExistsException
				=> (409, "Relationship between companies already exists"),
			EmailAlreadyExistsException
				=> (409, "Email address is already taken"),
			PhoneAlreadyExistsException
				=> (409, "Phone number is already taken"),
			ParticipantAlreadyAssignedException
				=> (409, "Participant is already assigned to this event"),
			ParticipantTimeConflictException
				=> (409, "Participant has another event scheduled at this time"),
			StaffMemberTimeConflictException
				=> (409, "Staff member has another event scheduled at this time"),
			StaffMemberSpecializationAlreadyAssignedException
				=> (409, "Staff member already has this specialization assigned"),
			MaxParticipantsExceededException
				=> (409, "Maximum number of participants has been reached"),
			_ => (500, "An unexpected server error occurred")
		};

		context.Response.StatusCode = statusCode;
		object response = _environment.IsDevelopment()
			? new
			{
				statusCode,
				userMessage,
				details = GetExceptionDetails(exception)
			}
			: new { statusCode, userMessage };

		await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
	}

	private static IEnumerable<object> GetExceptionDetails(Exception ex)
	{
		List<object> details = [];
		Exception? current = ex;
		while (current is not null)
		{
			details.Add(new
			{
				type = current.GetType().Name,
				message = current.Message,
				stackTrace = current.StackTrace?
					.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
			});

			current = current.InnerException;
		}

		return details;
	}
}