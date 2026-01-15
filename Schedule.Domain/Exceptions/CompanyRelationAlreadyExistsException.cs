namespace Schedule.Domain.Exceptions;

public class CompanyRelationAlreadyExistsException : Exception
{
	public CompanyRelationAlreadyExistsException(
		Guid childId,
		Guid parentId)
		: base($"Relation between companies {childId} and {parentId} already exists")
	{
	}
}