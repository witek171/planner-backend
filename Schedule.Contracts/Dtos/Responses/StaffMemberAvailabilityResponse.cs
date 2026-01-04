using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class StaffMemberAvailabilityResponse
{
	[Required] public StaffMemberResponse StaffMember { get; }
	[Required] public IReadOnlyList<AvailableSlotResponse> AvailableSlots { get; }

	public StaffMemberAvailabilityResponse(
		StaffMemberResponse staffMember,
		IReadOnlyList<AvailableSlotResponse> availableSlots)
	{
		StaffMember = staffMember;
		AvailableSlots = availableSlots;
	}
}