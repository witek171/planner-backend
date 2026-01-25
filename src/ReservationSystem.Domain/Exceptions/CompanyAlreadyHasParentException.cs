namespace ReservationSystem.Domain.Exceptions;

public class CompanyAlreadyHasParentException : Exception
{
	public CompanyAlreadyHasParentException(Guid childId)
		: base($"Company {childId} already has a parent company")
	{
	}
}