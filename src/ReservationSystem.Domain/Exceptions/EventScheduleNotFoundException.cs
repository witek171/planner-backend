namespace ReservationSystem.Domain.Exceptions;

public class EventScheduleNotFoundException : Exception
{
	public EventScheduleNotFoundException(Guid eventScheduleId)
		: base($"Event ReservationSystem {eventScheduleId} not found")
	{
	}
}