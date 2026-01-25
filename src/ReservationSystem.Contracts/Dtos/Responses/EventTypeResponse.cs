using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Responses;

public class EventTypeResponse
{
	[Required] public Guid Id { get; init; }
	[Required] public string Name { get; init; }
	[Required] public string Description { get; init; }
	[Required] public int Duration { get; init; }
	[Required] public decimal Price { get; init; }
	[Required] public int MaxParticipants { get; init; }
	[Required] public int MinStaff { get; init; }
}