namespace ReservationSystem.Domain.Exceptions;

public class ParticipantAlreadyAssignedException : Exception
{
	public ParticipantAlreadyAssignedException(Guid participantId, Guid eventScheduleId)
		: base($"Participant {participantId} is already assigned to event schedule {eventScheduleId}")
	{
	}
}