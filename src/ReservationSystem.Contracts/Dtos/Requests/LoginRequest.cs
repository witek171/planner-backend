using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class LoginRequest
{
	[Required, EmailAddress] public string Email { get; init; }
	[Required] public string Password { get; init; }
}