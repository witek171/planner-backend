namespace Schedule.Domain.Exceptions;

public class InvalidBreakTimeStaffException : Exception
{
	public InvalidBreakTimeStaffException()
		: base("Break time for staff must be equal or greater than zero")
	{
	}
}