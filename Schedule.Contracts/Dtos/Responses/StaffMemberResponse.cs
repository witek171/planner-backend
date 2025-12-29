using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class StaffMemberResponse
{
	public StaffMemberResponse(
		Guid id,
		string role,
		string email,
		string firstName,
		string lastName,
		string phone,
		List<SpecializationResponse> specializations)
	{
		Id = id;
		Role = role;
		Email = email;
		FirstName = firstName;
		LastName = lastName;
		Phone = phone;
		Specializations = specializations;
	}

	[Required] public Guid Id { get; }
	[Required] public string Role { get; }
	[Required] public string Email { get; }
	[Required] public string FirstName { get; }
	[Required] public string LastName { get; }
	[Required] public string Phone { get; }
	[Required] public List<SpecializationResponse> Specializations { get; }
}