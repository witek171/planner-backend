using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Application.Interfaces.Validators;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class ReservationService : IReservationService
{
	private readonly IReservationRepository _reservationRepository;
	private readonly IReservationParticipantRepository _reservationParticipantRepository;
	private readonly IEventScheduleRepository _eventScheduleRepository;
	private readonly IParticipantRepository _participantRepository;
	private readonly IScheduleConflictValidator _scheduleConflictValidator;

	public ReservationService(
		IReservationRepository reservationRepository,
		IReservationParticipantRepository reservationParticipantRepository,
		IEventScheduleRepository eventScheduleRepository,
		IParticipantRepository participantRepository,
		IScheduleConflictValidator scheduleConflictValidator)
	{
		_reservationRepository = reservationRepository;
		_reservationParticipantRepository = reservationParticipantRepository;
		_eventScheduleRepository = eventScheduleRepository;
		_participantRepository = participantRepository;
		_scheduleConflictValidator = scheduleConflictValidator;
	}

	public async Task<(List<Reservation> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize)
		=> await _reservationRepository.GetPagedWithCountAsync(companyId, page, pageSize);

	public async Task<Reservation?> GetByIdAsync(
		Guid id,
		Guid companyId)
		=> await _reservationRepository.GetByIdAsync(id, companyId);

	public async Task<Guid> CreateAsync(Reservation reservation)
	{
		await ValidateEventScheduleAsync(reservation);
		await ValidateParticipantsAsync(reservation);
		IReadOnlyList<Guid> participantsIds = reservation.ParticipantsIds;
		if (participantsIds.Count != participantsIds.Distinct().Count())
			throw new InvalidOperationException("Duplicate participants found in the list");

		Guid companyId = reservation.CompanyId;
		Guid eventScheduleId = reservation.EventScheduleId;
		EventSchedule eventSchedule = (await _eventScheduleRepository
			.GetByIdAsync(eventScheduleId, companyId))!;
		DateTime startTime = eventSchedule.StartTime;
		DateTime endTime = eventSchedule.EndTime;
		foreach (Guid participantId in participantsIds)
		{
			bool isParticipantAssigned = await _eventScheduleRepository
				.IsParticipantAssignedAsync(participantId, eventScheduleId);
			if (isParticipantAssigned)
				throw new InvalidOperationException(
					$"Participant {participantId} is already assigned to event schedule {eventScheduleId}");

			if (!await _scheduleConflictValidator
					.CanAssignParticipantAsync(companyId, participantId, startTime, endTime))
				throw new InvalidOperationException(
					$"Participant {participantId} has a time conflict");
		}

		int participantsCount = reservation.ParticipantsIds.Count;
		(int maxParticipants, int currentParticipants) = await _eventScheduleRepository
			.GetMaxParticipantsAndCurrentParticipantsAsync(eventScheduleId, companyId);

		if (participantsCount + currentParticipants > maxParticipants)
			throw new InvalidOperationException(
				$"The reservation cannot be created. The maximum number of participants for this " +
				$"event is {maxParticipants}, but {currentParticipants} are already " +
				$"registered and you are trying to add {participantsCount} more");

		reservation.Normalize();
		if (reservation.IsPaid)
			reservation.InitializePaidAt();

		Guid reservationId = await _reservationRepository.CreateAsync(reservation);

		foreach (Guid participantId in participantsIds)
		{
			ReservationParticipant reservationParticipant =
				new(Guid.Empty, companyId, reservationId, participantId);
			await _reservationParticipantRepository.CreateAsync(reservationParticipant);
		}

		return reservationId;
	}

	public async Task UpdateAsync(Reservation reservation)
	{
		reservation.Normalize();
		await _reservationRepository.UpdateAsync(reservation);
	}

	public async Task SoftDeleteAsync(
		Guid id,
		Guid companyId)
	{
		Reservation reservation = (await _reservationRepository.GetByIdAsync(id, companyId))!;

		reservation.SoftDelete();
		await _reservationRepository.UpdateSoftDeleteAsync(reservation);
	}

	public async Task MarkAsPaidAsync(Reservation reservation)
	{
		reservation.MarkAsPaid();
		await _reservationRepository.UpdatePaymentDetailsAsync(reservation);
	}

	public async Task UnmarkAsPaidAsync(Reservation reservation)
	{
		reservation.UnmarkAsPaid();
		await _reservationRepository.UpdatePaymentDetailsAsync(reservation);
	}

	private async Task ValidateEventScheduleAsync(Reservation reservation)
	{
		Guid eventScheduleId = reservation.EventScheduleId;
		Guid companyId = reservation.CompanyId;

		EventSchedule? eventType = await _eventScheduleRepository
			.GetByIdAsync(eventScheduleId, companyId);
		if (eventType == null)
			throw new InvalidOperationException(
				$"Event schedule {eventScheduleId} not found");
	}

	private async Task ValidateParticipantsAsync(Reservation reservation)
	{
		IReadOnlyList<Guid> participantsIds = reservation.ParticipantsIds;
		Guid companyId = reservation.CompanyId;

		foreach (Guid participantId in participantsIds)
		{
			Participant? participant = await _participantRepository
				.GetByIdAsync(participantId, companyId);
			if (participant == null)
				throw new InvalidOperationException(
					$"Participant {participantId} not found");
		}
	}
}