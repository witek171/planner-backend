using ReservationSystem.Domain.Models.Enums;

namespace ReservationSystem.Domain.Models;

public class EventSchedule
{
	public Guid Id { get; }
	public Guid CompanyId { get; private set; }
	public Guid EventTypeId { get; private set; }
	public EventType EventType { get; private set; }
	public string PlaceName { get; private set; }
	public DateTime StartTime { get; private set; }
	public DateTime EndTime => StartTime.AddMinutes(EventType.Duration);
	public DateTime CreatedAt { get; }
	public EventScheduleStatus Status { get; private set; }

	public EventSchedule(
		Guid id,
		Guid companyId,
		Guid eventTypeId,
		EventType eventType,
		string placeName,
		DateTime startTime,
		DateTime createdAt,
		EventScheduleStatus status)
	{
		Id = id;
		CompanyId = companyId;
		EventTypeId = eventTypeId;
		EventType = eventType;
		PlaceName = placeName;
		StartTime = startTime;
		CreatedAt = createdAt;
		Status = status;
	}

	public EventSchedule()
	{
	}

	public void SetCompanyId(Guid companyId)
	{
		if (CompanyId != Guid.Empty)
			throw new InvalidOperationException(
				$"CompanyId is already set to {CompanyId} and cannot be changed");

		CompanyId = companyId;
	}

	public void Normalize()
		=> PlaceName = PlaceName.Trim();

	public void SoftDelete()
	{
		if (Status == EventScheduleStatus.Deleted)
			throw new InvalidOperationException(
				$"Event {Id} is already marked as deleted");

		Status = EventScheduleStatus.Deleted;
	}

	public void SetAsActive()
	{
		if (Status == EventScheduleStatus.Active)
			throw new InvalidOperationException(
				$"Event {Id} is already marked as Active");

		Status = EventScheduleStatus.Active;
	}
}