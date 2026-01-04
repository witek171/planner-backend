using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class EventScheduleResponse
{
	[Required] public Guid Id { get; init; }
	[Required] public EventTypeResponse EventType { get; init; }
	[Required] public string PlaceName { get; init; }
	[Required] public DateTime StartTime { get; init; }
	[Required] public DateTime EndTime => StartTime.AddMinutes(EventType.Duration);
	[Required] public DateTime CreatedAt { get; init; }
	[Required] public string Status { get; init; }
}