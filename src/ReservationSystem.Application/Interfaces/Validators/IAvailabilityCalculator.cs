using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Validators;

public interface IAvailabilityCalculator
{
	Task<List<StaffMemberAvailability>> CalculateAvailableTimeSlots(
		List<StaffMemberAvailability> staffMemberAvailabilities,
		List<EventSchedule> staffMemberEvents,
		Guid companyId);
}