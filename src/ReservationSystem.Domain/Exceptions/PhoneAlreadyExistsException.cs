namespace ReservationSystem.Domain.Exceptions;

public class PhoneAlreadyExistsException : Exception
{
	public PhoneAlreadyExistsException(string phone, Guid companyId)
		: base($"Phone {phone} already exists for company {companyId}")
	{
	}

	public PhoneAlreadyExistsException(string phone)
		: base($"Phone {phone} already exists")
	{
	}
}