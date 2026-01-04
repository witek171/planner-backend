using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Requests;

public class ReservationUpdateRequest
{
	[Required] public string Notes { get; init; }
}