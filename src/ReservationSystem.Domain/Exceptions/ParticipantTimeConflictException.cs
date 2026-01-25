namespace ReservationSystem.Domain.Exceptions;

public class ParticipantTimeConflictException : Exception
{
	public ParticipantTimeConflictException(Guid participantId)
		: base($"Participant {participantId} has a time conflict")
	{
	}
}