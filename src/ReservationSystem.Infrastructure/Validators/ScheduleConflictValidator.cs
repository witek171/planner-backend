using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Application.Interfaces.Validators;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Infrastructure.Validators;

using Itenso.TimePeriod;

public class ScheduleConflictValidator : IScheduleConflictValidator
{
	private readonly IReservationRepository _reservationRepository;
	private readonly ICompanyConfigRepository _companyConfigRepository;
	private readonly IStaffMemberAvailabilityService _staffMemberAvailabilityService;

	public ScheduleConflictValidator(
		IReservationRepository reservationRepository,
		ICompanyConfigRepository companyConfigRepository,
		IStaffMemberAvailabilityService staffMemberAvailabilityService)
	{
		_reservationRepository = reservationRepository;
		_companyConfigRepository = companyConfigRepository;
		_staffMemberAvailabilityService = staffMemberAvailabilityService;
	}

	public async Task<bool> CanAssignStaffMemberAsync(
		Guid companyId,
		Guid staffMemberId,
		DateTime start,
		DateTime end)
	{
		TimeRange eventScheduleRange = new(start, end);
		List<StaffMemberAvailability> availabilities = await _staffMemberAvailabilityService
			.GetByStaffMemberIdAsync(companyId, staffMemberId);

		foreach (StaffMemberAvailability availability in availabilities)
		{
			DateTime startTime = availability.StartTime;
			DateTime endTime = availability.EndTime;
			TimeRange availabilityRange = new(startTime, endTime);
			if (availabilityRange.HasInside(eventScheduleRange))
				return true;
		}

		return false;
	}

	public async Task<bool> CanAssignParticipantAsync(
		Guid companyId,
		Guid participantId,
		DateTime start,
		DateTime end)
	{
		CompanyConfig companyConfig = (await _companyConfigRepository.GetByIdAsync(companyId))!;
		int breakTime = companyConfig.BreakTimeParticipants;

		TimeRange bufferedEventScheduleRange = new(start.AddMinutes(-breakTime), end.AddMinutes(breakTime));
		List<Reservation> reservations = await _reservationRepository
			.GetByParticipantIdAsync(companyId, participantId);

		foreach (Reservation reservation in reservations)
		{
			DateTime startTime = reservation.EventSchedule.StartTime;
			DateTime endTime = reservation.EventSchedule.EndTime;
			TimeRange existingRange = new(startTime, endTime);
			if (bufferedEventScheduleRange.IntersectsWith(existingRange))
				return false;
		}

		return true;
	}
}