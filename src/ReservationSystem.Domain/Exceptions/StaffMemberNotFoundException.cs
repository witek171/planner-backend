namespace ReservationSystem.Domain.Exceptions;

public class StaffMemberNotFoundException : Exception
{
	public StaffMemberNotFoundException(Guid staffMemberId)
		: base($"Staff member {staffMemberId} not found")
	{
	}
}