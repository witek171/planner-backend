using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Domain.Exceptions;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class EventScheduleService : IEventScheduleService
{
	private readonly IEventScheduleRepository _eventScheduleRepository;
	private readonly IReservationRepository _reservationRepository;
	private readonly IEventTypeRepository _eventTypeRepository;
	private readonly IReservationService _reservationService;

	public EventScheduleService(
		IEventScheduleRepository eventScheduleRepository,
		IReservationRepository reservationRepository,
		IEventTypeRepository eventTypeRepository,
		IReservationService reservationService)
	{
		_eventScheduleRepository = eventScheduleRepository;
		_reservationRepository = reservationRepository;
		_eventTypeRepository = eventTypeRepository;
		_reservationService = reservationService;
	}

	public async Task<List<EventSchedule>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId)
		=> await _eventScheduleRepository.GetByStaffMemberIdAsync(companyId, staffMemberId);

	public async Task<(List<EventSchedule> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize)
		=> await _eventScheduleRepository.GetPagedWithCountAsync(companyId, page, pageSize);

	public async Task<EventSchedule?> GetByIdAsync(
		Guid id,
		Guid companyId)
		=> await _eventScheduleRepository.GetByIdAsync(id, companyId);

	public async Task<Guid> CreateAsync(EventSchedule eventSchedule)
	{
		await ValidateEventTypeAsync(eventSchedule);
		eventSchedule.Normalize();
		return await _eventScheduleRepository.CreateAsync(eventSchedule);
	}

	public async Task UpdateAsync(EventSchedule eventSchedule)
	{
		await ValidateEventTypeAsync(eventSchedule);
		eventSchedule.Normalize();
		await _eventScheduleRepository.UpdateAsync(eventSchedule);
	}

	public async Task DeleteAsync(
		Guid id,
		Guid companyId)
	{
		if (await _eventScheduleRepository.HasRelatedRecordsAsync(id, companyId))
		{
			// wyslanie powiadomienia o odwolaniu zajec do pracownika
			// wyslanie powiadomienia o odwolaniu zajec do uczestnika

			List<Guid> reservationIds = await _reservationRepository
				.GetIdsByEventScheduleIdAsync(id, companyId);
			foreach (Guid reservationId in reservationIds)
				await _reservationService.SoftDeleteAsync(reservationId, companyId);

			EventSchedule eventSchedule = (await _eventScheduleRepository.GetByIdAsync(id, companyId))!;
			eventSchedule.SoftDelete();
			await _eventScheduleRepository.UpdateStatusAsync(eventSchedule);
		}
		else
			await _eventScheduleRepository.DeleteAsync(id, companyId);
	}

	private async Task ValidateEventTypeAsync(EventSchedule eventSchedule)
	{
		Guid eventTypeId = eventSchedule.EventTypeId;
		Guid companyId = eventSchedule.CompanyId;

		EventType? eventType = await _eventTypeRepository
			.GetByIdAsync(eventTypeId, companyId);
		if (eventType == null)
			throw new EventTypeNotFoundException(eventTypeId);
	}
}