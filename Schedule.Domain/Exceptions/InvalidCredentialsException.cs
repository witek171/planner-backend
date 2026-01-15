namespace Schedule.Domain.Exceptions;

public class InvalidCredentialsException : Exception
{
	public InvalidCredentialsException() : base("Incorrect password or email")
	{
	}
}