using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class ParticipantUpdateRequest
{
	[Required, EmailAddress] public string Email { get; init; }
	[Required, StringLength(40)] public string FirstName { get; init; }
	[Required, StringLength(40)] public string LastName { get; init; }
	[Required, Phone] public string Phone { get; init; }
}