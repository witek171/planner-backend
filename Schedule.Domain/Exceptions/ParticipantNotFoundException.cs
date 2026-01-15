namespace Schedule.Domain.Exceptions;

public class ParticipantNotFoundException : Exception
{
	public ParticipantNotFoundException(Guid participantId)
		: base($"Participant {participantId} not found")
	{
	}
}