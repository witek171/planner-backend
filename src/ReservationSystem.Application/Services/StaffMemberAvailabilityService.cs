using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Application.Interfaces.Validators;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Services;

public class StaffMemberAvailabilityService : IStaffMemberAvailabilityService
{
	private readonly IStaffMemberAvailabilityRepository _staffMemberAvailabilityRepository;
	private readonly IEventScheduleRepository _eventScheduleRepository;
	private readonly IAvailabilityCalculator _availabilityCalculator;

	public StaffMemberAvailabilityService(
		IStaffMemberAvailabilityRepository staffMemberAvailabilityRepository,
		IEventScheduleRepository eventScheduleRepository,
		IAvailabilityCalculator availabilityCalculator)
	{
		_staffMemberAvailabilityRepository = staffMemberAvailabilityRepository;
		_eventScheduleRepository = eventScheduleRepository;
		_availabilityCalculator = availabilityCalculator;
	}

	public async Task<List<StaffMemberAvailability>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId)
	{
		List<StaffMemberAvailability> staffMemberAvailabilities = await _staffMemberAvailabilityRepository
			.GetByStaffMemberIdAsync(companyId, staffMemberId);
		List<EventSchedule> staffMemberEvents = await _eventScheduleRepository
			.GetByStaffMemberIdAsync(companyId, staffMemberId);

		return await _availabilityCalculator
			.CalculateAvailableTimeSlots(staffMemberAvailabilities, staffMemberEvents, companyId);
	}

	public async Task<Guid> CreateAsync(StaffMemberAvailability availability)
	{
		availability.MarkAsAvailable();
		return await _staffMemberAvailabilityRepository.CreateAsync(availability);
	}

	public async Task DeleteAsync(
		Guid companyId,
		Guid id)
		=> await _staffMemberAvailabilityRepository.DeleteByIdAsync(companyId, id);

	public async Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id)
		=> await _staffMemberAvailabilityRepository.ExistsByIdAsync(companyId, id);
}