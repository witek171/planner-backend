namespace ReservationSystem.Domain.Models;

public class Participant
{
	public Guid Id { get; }
	public Guid CompanyId { get; private set; }
	public string Email { get; private set; }
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public string Phone { get; private set; }
	public bool GdprConsent { get; private set; }
	public DateTime CreatedAt { get; }

	public Participant()
	{
	}

	public Participant(
		Guid id,
		Guid receptionId,
		string email,
		string firstName,
		string lastName,
		string phone,
		bool gdprConsent,
		DateTime createdAt)
	{
		Id = id;
		CompanyId = receptionId;
		Email = email;
		FirstName = firstName;
		LastName = lastName;
		Phone = phone;
		GdprConsent = gdprConsent;
		CreatedAt = createdAt;
	}

	public void SetCompanyId(Guid companyId)
	{
		if (CompanyId != Guid.Empty)
			throw new InvalidOperationException(
				$"CompanyId is already set to {CompanyId} and cannot be changed");

		CompanyId = companyId;
	}

	public void Normalize()
	{
		Email = Email.Trim().ToLowerInvariant();
		FirstName = FirstName.Trim();
		LastName = LastName.Trim();
		Phone = Phone.Trim();
	}

	public void Anonymize()
	{
		Email = "(deleted)";
		LastName = LastName[0] + " (deleted)";
		Phone = "(deleted)";
	}
}