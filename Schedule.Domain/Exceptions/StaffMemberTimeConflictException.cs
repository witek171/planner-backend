namespace Schedule.Domain.Exceptions;

public class StaffMemberTimeConflictException : Exception
{
	public StaffMemberTimeConflictException(Guid staffMemberId)
		: base($"Staff member {staffMemberId} has a time conflict")
	{
	}
}