namespace ReservationSystem.Domain.Models;

public class StaffMemberCompany
{
	public Guid Id { get; private set; }

	public Guid StaffMemberId { get; private set; }
	public Guid CompanyId { get; private set; }

	public DateTime CreatedAt { get; private set; }

	private StaffMemberCompany() { }

	public StaffMemberCompany(Guid id, Guid staffMemberId, Guid companyId, DateTime createdAt)
	{
		Id = id;
		StaffMemberId = staffMemberId;
		CompanyId = companyId;
		CreatedAt = createdAt;
	}
}