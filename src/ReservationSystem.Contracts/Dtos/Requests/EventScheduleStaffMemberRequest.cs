using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class EventScheduleStaffMemberRequest
{
	[Required] public Guid EventScheduleId { get; init; }
	[Required] public Guid StaffMemberId { get; init; }
}