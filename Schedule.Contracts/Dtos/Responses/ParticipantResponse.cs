using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class ParticipantResponse
{
	public ParticipantResponse(
		Guid id,
		string email,
		string firstName,
		string lastName,
		string phone,
		DateTime createdAt)
	{
		Id = id;
		Email = email;
		FirstName = firstName;
		LastName = lastName;
		Phone = phone;
		CreatedAt = createdAt;
	}

	[Required] public Guid Id { get; }
	[Required] public string Email { get; }
	[Required] public string FirstName { get; }
	[Required] public string LastName { get; }
	[Required] public string Phone { get; }
	[Required] public DateTime CreatedAt { get; }
}