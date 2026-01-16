namespace Schedule.Domain.Exceptions;

public class StaffMemberSpecializationAlreadyAssignedException : Exception
{
	public StaffMemberSpecializationAlreadyAssignedException(Guid staffMemberId, Guid specializationId)
		: base($"Staff member {staffMemberId} already has specialization {specializationId} assigned")
	{
	}
}