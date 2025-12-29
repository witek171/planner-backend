using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class AvailabilityResponse
{
	public AvailabilityResponse(
		Guid id,
		DateOnly date,
		DateTime startTime,
		DateTime endTime)
	{
		Id = id;
		Date = date;
		StartTime = startTime;
		EndTime = endTime;
	}

	[Required] public Guid Id { get; }
	[Required] public DateOnly Date { get; }
	[Required] public DateTime StartTime { get; }
	[Required] public DateTime EndTime { get; }
}