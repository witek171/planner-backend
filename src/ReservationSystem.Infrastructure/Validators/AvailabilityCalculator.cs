using Itenso.TimePeriod;
using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Application.Interfaces.Validators;
using ReservationSystem.Domain.Models;
using ReservationSystem.Domain.Models.Enums;

namespace ReservationSystem.Infrastructure.Validators;

public class AvailabilityCalculator : IAvailabilityCalculator
{
	private readonly ICompanyConfigRepository _companyConfigRepository;

	public AvailabilityCalculator(ICompanyConfigRepository companyConfigRepository)
	{
		_companyConfigRepository = companyConfigRepository;
	}

	public async Task<List<StaffMemberAvailability>> CalculateAvailableTimeSlots(
		List<StaffMemberAvailability> staffMemberAvailabilities,
		List<EventSchedule> staffMemberEvents,
		Guid companyId)
	{
		CompanyConfig companyConfig = (await _companyConfigRepository.GetByIdAsync(companyId))!;
		int breakTime = companyConfig.BreakTimeStaff;

		List<StaffMemberAvailability> freeTimeSlots = new();
		TimePeriodSubtractor<TimeRange> subtractor = new();
		TimePeriodCombiner<TimeRange> combiner = new();

		foreach (StaffMemberAvailability availability in staffMemberAvailabilities)
		{
			TimePeriodCollection availabilityPeriod =
			[
				new TimeRange(availability.StartTime, availability.EndTime)
			];

			TimePeriodCollection blockedPeriods = new();

			foreach (EventSchedule eventSchedule in
					staffMemberEvents.Where(e => e.Status == EventScheduleStatus.Active))
			{
				DateTime blockedStart = eventSchedule.StartTime.AddMinutes(-breakTime);
				DateTime blockedEnd = eventSchedule.EndTime.AddMinutes(breakTime);
				blockedPeriods.Add(new TimeRange(blockedStart, blockedEnd));
			}

			ITimePeriodCollection? mergedBlockedPeriods = combiner.CombinePeriods(blockedPeriods);

			ITimePeriodCollection availablePeriods =
				subtractor.SubtractPeriods(availabilityPeriod, mergedBlockedPeriods);

			foreach (ITimePeriod freeSlot in availablePeriods)
				if (freeSlot.Duration > TimeSpan.Zero)
					freeTimeSlots.Add(new StaffMemberAvailability(
						Guid.Empty,
						availability.CompanyId,
						availability.StaffMemberId,
						DateOnly.FromDateTime(freeSlot.Start),
						freeSlot.Start,
						freeSlot.End,
						true));
		}

		return freeTimeSlots;
	}
}