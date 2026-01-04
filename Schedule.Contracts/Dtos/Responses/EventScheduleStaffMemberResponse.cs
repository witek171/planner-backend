using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class EventScheduleStaffMemberResponse
{
	[Required] public Guid EventScheduleId { get; init; }
	[Required] public Guid StaffMemberId { get; init; }
}