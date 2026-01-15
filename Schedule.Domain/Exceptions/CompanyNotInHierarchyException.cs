namespace Schedule.Domain.Exceptions;

public class CompanyNotInHierarchyException : Exception
{
	public CompanyNotInHierarchyException(Guid companyId)
		: base($"Company {companyId} is not present in the hierarchy, therefore it has no relations to remove")
	{
	}
}