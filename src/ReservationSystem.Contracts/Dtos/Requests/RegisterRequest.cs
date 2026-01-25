using ReservationSystem.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

internal class RegisterRequest
{
	[Required] public StaffRole Role { get; init; }
	[Required, EmailAddress] public string Email { get; init; }
	[Required] public string Password { get; init; }
	[Required, MaxLength(40)] public string FirstName { get; init; }
	[Required, MaxLength(40)] public string LastName { get; init; }
	[Required, Phone] public string Phone { get; init; }
}