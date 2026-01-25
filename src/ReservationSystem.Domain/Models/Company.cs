namespace ReservationSystem.Domain.Models;

public class Company
{
	public Guid Id { get; }
	public string Name { get; private set; }
	public string TaxCode { get; private set; }
	public string Street { get; private set; }
	public string City { get; private set; }
	public string PostalCode { get; private set; }
	public string Phone { get; private set; }
	public string Email { get; private set; }
	public bool IsParentNode { get; private set; }
	public bool IsReception { get; private set; }
	public DateTime CreatedAt { get; }

	public Company()
	{
	}

	public Company(
		Guid id,
		string name,
		string taxCode,
		string street,
		string city,
		string postalCode,
		string phone,
		string email,
		bool isParentNode,
		bool isReception,
		DateTime createdAt)
	{
		Id = id;
		Name = name;
		TaxCode = taxCode;
		Street = street;
		City = city;
		PostalCode = postalCode;
		Phone = phone;
		Email = email;
		IsParentNode = isParentNode;
		IsReception = isReception;
		CreatedAt = createdAt;
	}

	public void Normalize()
	{
		Name = Name.Trim();
		TaxCode = TaxCode.Trim();
		Street = Street.Trim();
		City = City.Trim();
		PostalCode = PostalCode.Trim();
		Phone = Phone.Trim();
		Email = Email.Trim().ToLowerInvariant();
	}

	public void MarkAsParentNode()
	{
		if (IsParentNode)
			throw new InvalidOperationException(
				$"Company {Id} is already marked as the main company");

		IsParentNode = true;
	}

	public void UnmarkAsParentNode()
	{
		if (!IsParentNode)
			throw new InvalidOperationException(
				$"Company {Id} is currently not marked as the main company");
		
		IsParentNode = false;
	}

	public void MarkAsReception()
	{
		if (IsReception)
			throw new InvalidOperationException(
				$"Company {Id} is already marked as a reception");

		IsReception = true;
	}

	public void UnmarkAsReception()
	{
		if (!IsReception)
			throw new InvalidOperationException(
				$"Company {Id} is currently not marked as a reception");

		IsReception = false;
	}
}