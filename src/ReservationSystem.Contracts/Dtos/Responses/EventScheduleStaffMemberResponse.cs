using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Responses;

public class EventScheduleStaffMemberResponse
{
	[Required] public Guid EventScheduleId { get; init; }
	[Required] public Guid StaffMemberId { get; init; }
}