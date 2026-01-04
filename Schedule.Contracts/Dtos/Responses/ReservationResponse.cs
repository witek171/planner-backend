using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class ReservationResponse
{
	[Required] public Guid Id { get; init; }
	[Required] public EventScheduleResponse EventSchedule { get; init; }
	[Required] public IReadOnlyList<ParticipantResponse> Participants { get; init; } = [];
	[Required] public string Status { get; init; }
	[Required] public string Notes { get; init; }
	[Required] public DateTime CreatedAt { get; init; }
	public DateTime? CancelledAt { get; init; }
	[Required] public bool IsPaid { get; init; }
	public DateTime? PaidAt { get; init; }
}