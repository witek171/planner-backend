namespace ReservationSystem.Domain.Exceptions;

public class EmailAlreadyExistsException : Exception
{
	public EmailAlreadyExistsException(string email, Guid companyId)
		: base($"Email {email} already exists for company {companyId}")
	{
	}

	public EmailAlreadyExistsException(string email)
		: base($"Email {email} already exists")
	{
	}
}