namespace ReservationSystem.Domain.Models;

public class Specialization
{
	public Specialization()
	{

	}

	public Specialization(
		Guid id,
		Guid companyId,
		string name,
		string description)
	{
		Id = id;
		CompanyId = companyId;
		Name = name;
		Description = description;
	}

	public Guid Id { get; }
	public Guid CompanyId { get; private set; }
	public string Name { get; private set; }
	public string Description { get; private set; }

	public void SetCompanyId(Guid companyId)
	{
		if (CompanyId != Guid.Empty)
			throw new InvalidOperationException(
				$"CompanyId is already set to {CompanyId} and cannot be changed");

		CompanyId = companyId;
	}
}