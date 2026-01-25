namespace ReservationSystem.Domain.Exceptions;

public class EventTypeNotFoundException : Exception
{
	public EventTypeNotFoundException(Guid eventTypeId)
		: base($"Event type {eventTypeId} not found")
	{
	}
}