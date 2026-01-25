using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class CompanyRequest
{
	[Required] public string Name { get; init; }
	[Required] public string TaxCode { get; init; }
	[Required] public string Street { get; init; }
	[Required] public string City { get; init; }
	[Required] public string PostalCode { get; init; }
	[Required, Phone] public string Phone { get; init; }
	[Required, EmailAddress] public string Email { get; init; }
}