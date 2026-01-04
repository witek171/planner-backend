using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Requests;

public class StaffMemberAvailabilityRequest
{
	[Required] public DateOnly Date { get; init; }
	[Required] public DateTime StartTime { get; init; }
	[Required] public DateTime EndTime { get; init; }
}