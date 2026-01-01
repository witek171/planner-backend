using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class EventTypeService : IEventTypeService
{
	private readonly IEventTypeRepository _eventTypeRepository;

	public EventTypeService(IEventTypeRepository eventTypeRepository)
	{
		_eventTypeRepository = eventTypeRepository;
	}

	public async Task<(List<EventType> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize)
		=> await _eventTypeRepository.GetPagedWithCountAsync(companyId, page, pageSize);

	public async Task<EventType?> GetByIdAsync(
		Guid id,
		Guid companyId)
		=> await _eventTypeRepository.GetByIdAsync(id, companyId);

	public async Task<Guid> CreateAsync(EventType eventType)
	{
		eventType.Normalize();
		return await _eventTypeRepository.CreateAsync(eventType);
	}

	public async Task UpdateAsync(EventType eventType)
	{
		eventType.Normalize();
		await _eventTypeRepository.UpdateAsync(eventType);
	}

	public async Task DeleteAsync(
		Guid id,
		Guid companyId)
	{
		if (await _eventTypeRepository.ExistsInEventSchedulesAsync(id, companyId))
		{
			EventType eventType = (await _eventTypeRepository.GetByIdAsync(id, companyId))!;
			eventType.SoftDelete();
			await _eventTypeRepository.UpdateSoftDeleteAsync(eventType);
		}
		else
			await _eventTypeRepository.DeleteAsync(id, companyId);
	}
}