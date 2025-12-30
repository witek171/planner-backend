using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class StaffMemberAvailabilityResponse
{
	public StaffMemberAvailabilityResponse(
		StaffMemberResponse staffMember,
		List<AvailabilityResponse> availableSlots)
	{
		StaffMember = staffMember;
		AvailableSlots = availableSlots;
	}

	[Required] public StaffMemberResponse StaffMember { get; }
	[Required] public List<AvailabilityResponse> AvailableSlots { get; }
}