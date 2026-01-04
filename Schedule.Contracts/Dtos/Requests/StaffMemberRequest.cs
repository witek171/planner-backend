using System.ComponentModel.DataAnnotations;
using Schedule.Domain.Models.Enums;

namespace Schedule.Contracts.Dtos.Requests;

public class StaffMemberRequest
{
	[Required] public StaffRole Role { get; init; }
	[Required, EmailAddress] public string Email { get; init; }
	[Required] public string Password { get; init; }
	[Required, MaxLength(40)] public string FirstName { get; init; }
	[Required, MaxLength(40)] public string LastName { get; init; }
	[Required, Phone] public string Phone { get; init; }
}