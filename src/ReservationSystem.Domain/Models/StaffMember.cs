using ReservationSystem.Domain.Models.Enums;

namespace ReservationSystem.Domain.Models;

public class StaffMember
{
	public Guid Id { get; }
	public StaffRole Role { get; }
	public string Email { get; private set; }
	public string Password { get; private set; }
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public string Phone { get; private set; }
	public DateTime CreatedAt { get; }
	public bool IsDeleted { get; private set; }
	public IReadOnlyList<Specialization> Specializations { get; private set; }
	public IReadOnlyList<StaffMemberCompany> StaffMemberCompanies { get; private set; }

	public IEnumerable<Guid> CompanyIds => StaffMemberCompanies.Select(sc => sc.CompanyId);

	public StaffMember()
	{
		Specializations = new List<Specialization>();
		StaffMemberCompanies = new List<StaffMemberCompany>();
	}

	public StaffMember(
		Guid id,
		StaffRole role,
		string email,
		string password,
		string firstName,
		string lastName,
		string phone,
		DateTime createdAt,
		bool isDeleted,
		List<Specialization> specializations,
		List<StaffMemberCompany> staffMemberCompanies)
	{
		Id = id;
		Role = role;
		Email = email;
		Password = password;
		FirstName = firstName;
		LastName = lastName;
		Phone = phone;
		CreatedAt = createdAt;
		IsDeleted = isDeleted;
		Specializations = specializations;
		StaffMemberCompanies = staffMemberCompanies;
	}

	public void Normalize()
	{
		Email = Email.Trim().ToLowerInvariant();
		FirstName = FirstName.Trim();
		LastName = LastName.Trim();
		Phone = Phone.Trim();
	}

	public void SetSpecializations(List<Specialization> specializations)
	{
		if (Specializations.Any())
			throw new InvalidOperationException(
				$"Specializations for staff member {Id} are already set and cannot be changed");

		Specializations = specializations;
	}

	public void AddCompany(Guid companyId)
	{
		if (StaffMemberCompanies.Any(sc => sc.CompanyId == companyId))
			return;

		List<StaffMemberCompany> companies = StaffMemberCompanies.ToList();
		companies.Add(new StaffMemberCompany(Guid.NewGuid(), Id, companyId, DateTime.UtcNow));
		StaffMemberCompanies = companies;
	}

	public void SoftDelete()
	{
		if (IsDeleted)
			throw new InvalidOperationException(
				$"Staff member {Id} is already marked as deleted");

		IsDeleted = true;
	}

	public void SetStaffMemberCompanies(List<StaffMemberCompany> staffMemberCompanies)
	{
		StaffMemberCompanies = staffMemberCompanies;
	}

	public void SetPassword(string hashedPassword)
	{
		if (string.IsNullOrWhiteSpace(hashedPassword))
			throw new ArgumentException("Password hash cannot be empty", nameof(hashedPassword));

		Password = hashedPassword;
	}
}
