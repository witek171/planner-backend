using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class CompanyResponse
{
	[Required] public Guid Id { get; init; }
	[Required] public string Name { get; init; }
	[Required] public string TaxCode { get; init; }
	[Required] public string Street { get; init; }
	[Required] public string City { get; init; }
	[Required] public string PostalCode { get; init; }
	[Required, Phone] public string Phone { get; init; }
	[Required, EmailAddress] public string Email { get; init; }
	[Required] public bool IsParentNode { get; init; }
	[Required] public bool IsReception { get; init; }
	[Required] public DateTime CreatedAt { get; init; }
}