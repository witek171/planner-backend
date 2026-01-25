using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class ReservationCreateRequest
{
	[Required] public Guid EventScheduleId { get; init; }
	[Required] public string Notes { get; init; }
	[Required] public IReadOnlyList<Guid> ParticipantsIds { get; init; }
	[Required] public bool IsPaid { get; init; }
}