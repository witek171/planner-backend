using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Responses;

public class AvailableSlotResponse
{
	[Required] public DateOnly Date { get; init; }
	[Required] public DateTime StartTime { get; init; }
	[Required] public DateTime EndTime { get; init; }
}