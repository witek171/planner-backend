namespace Schedule.Domain.Exceptions;

public class EventScheduleNotFoundException : Exception
{
	public EventScheduleNotFoundException(Guid eventScheduleId)
		: base($"Event Schedule {eventScheduleId} not found")
	{
	}
}