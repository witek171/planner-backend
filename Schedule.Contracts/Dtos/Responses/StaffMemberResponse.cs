using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class StaffMemberResponse
{
	[Required] public Guid Id { get; init; }
	[Required] public string Role { get; init; }
	[Required] public string Email { get; init; }
	[Required] public string FirstName { get; init; }
	[Required] public string LastName { get; init; }
	[Required] public string Phone { get; init; }

	[Required] public IReadOnlyList<SpecializationResponse> Specializations { get; init; } = [];
	// [Required] public List<CompanyResponse> Companies { get; init;} = []
}