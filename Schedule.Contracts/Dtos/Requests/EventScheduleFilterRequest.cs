namespace Schedule.Contracts.Dtos.Requests;

public class EventScheduleFilterRequest : PaginationRequest
{
	public Guid? EventTypeId { get; init; }
}