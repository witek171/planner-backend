using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Requests;

public class EventScheduleRequest
{
	[Required] public Guid EventTypeId { get; init; }
	[Required] public string PlaceName { get; init; }
	[Required] public DateTime StartTime { get; init; }
}