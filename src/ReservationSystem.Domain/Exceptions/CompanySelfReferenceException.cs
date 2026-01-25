namespace ReservationSystem.Domain.Exceptions;

public class CompanySelfReferenceException : Exception
{
	public CompanySelfReferenceException(Guid companyId)
		: base($"Company {companyId} cannot be its own parent")
	{
	}
}