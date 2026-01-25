namespace ReservationSystem.Domain.Models;

public class StaffMemberSpecialization
{
	public Guid Id { get; }
	public Guid CompanyId { get; private set; }
	public Guid StaffMemberId { get; private set; }
	public Guid SpecializationId { get; private set; }

	public StaffMemberSpecialization()
	{
	}

	public StaffMemberSpecialization(
		Guid id,
		Guid companyId,
		Guid staffMemberId,
		Guid specializationId)
	{
		Id = id;
		CompanyId = companyId;
		StaffMemberId = staffMemberId;
		SpecializationId = specializationId;
	}

	public void SetCompanyId(Guid companyId)
	{
		if (CompanyId != Guid.Empty)
			throw new InvalidOperationException(
				$"CompanyId is already set to {CompanyId} and cannot be changed");

		CompanyId = companyId;
	}
}