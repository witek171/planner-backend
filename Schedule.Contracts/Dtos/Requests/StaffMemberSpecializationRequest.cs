using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Requests;

public class StaffMemberSpecializationRequest
{
	[Required] public Guid StaffMemberId { get; init; }
	[Required] public Guid SpecializationId { get; init; }
}