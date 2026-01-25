using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class ReservationUpdateRequest
{
	[Required] public string Notes { get; init; }
}