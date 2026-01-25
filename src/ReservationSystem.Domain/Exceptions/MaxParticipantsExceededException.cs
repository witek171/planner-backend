namespace ReservationSystem.Domain.Exceptions;

public class MaxParticipantsExceededException : Exception
{
	public MaxParticipantsExceededException(int maxParticipants, int currentParticipants, int requestedParticipants)
		: base(
			$"The reservation cannot be created. The maximum number" +
			$" of participants for this event is {maxParticipants}, " +
			$"but {currentParticipants} are already registered and you " +
			$"are trying to add {requestedParticipants} more")
	{
	}
}