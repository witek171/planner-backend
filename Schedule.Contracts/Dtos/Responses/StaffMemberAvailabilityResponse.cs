using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class StaffMemberAvailabilityResponse
{
	public StaffMemberAvailabilityResponse(
		StaffMemberResponse staffMember,
		List<AvailableSlotResponse> availableSlots)
	{
		StaffMember = staffMember;
		AvailableSlots = availableSlots;
	}

	[Required] public StaffMemberResponse StaffMember { get; }
	[Required] public List<AvailableSlotResponse> AvailableSlots { get; }
}