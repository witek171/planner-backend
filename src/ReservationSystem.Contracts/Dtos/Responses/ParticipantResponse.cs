using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Responses;

public class ParticipantResponse
{
	[Required] public Guid Id { get; init; }
	[Required, EmailAddress] public string Email { get; init; }
	[Required] public string FirstName { get; init; }
	[Required] public string LastName { get; init; }
	[Required, Phone] public string Phone { get; init; }
	[Required] public DateTime CreatedAt { get; init; }
}