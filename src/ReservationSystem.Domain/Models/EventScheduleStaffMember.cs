namespace ReservationSystem.Domain.Models;

public class EventScheduleStaffMember
{
	public Guid Id { get; }
	public Guid CompanyId { get; private set; }
	public Guid EventScheduleId { get; private set; }
	public Guid StaffMemberId { get; private set; }

	public EventScheduleStaffMember()
	{
	}

	public EventScheduleStaffMember(
		Guid id,
		Guid companyId,
		Guid eventScheduleId,
		Guid staffMemberId)
	{
		Id = id;
		CompanyId = companyId;
		EventScheduleId = eventScheduleId;
		StaffMemberId = staffMemberId;
	}

	public void SetCompanyId(Guid companyId)
	{
		if (CompanyId != Guid.Empty)
			throw new InvalidOperationException(
				$"CompanyId is already set to {CompanyId} and cannot be changed");

		CompanyId = companyId;
	}
}