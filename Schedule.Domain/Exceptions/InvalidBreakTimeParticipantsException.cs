namespace Schedule.Domain.Exceptions;

public class InvalidBreakTimeParticipantsException : Exception
{
	public InvalidBreakTimeParticipantsException()
		: base("Break time for participants must be equal or greater than zero")
	{
	}
}