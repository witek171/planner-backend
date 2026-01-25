using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class StaffMemberSpecializationRequest
{
	[Required] public Guid StaffMemberId { get; init; }
	[Required] public Guid SpecializationId { get; init; }
}