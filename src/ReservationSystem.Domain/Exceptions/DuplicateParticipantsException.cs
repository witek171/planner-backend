namespace ReservationSystem.Domain.Exceptions;

public class DuplicateParticipantsException : Exception
{
	public DuplicateParticipantsException()
		: base("Duplicate participants found in the list")
	{
	}
}