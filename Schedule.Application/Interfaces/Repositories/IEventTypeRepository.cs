using Schedule.Domain.Models;

namespace Schedule.Application.Interfaces.Repositories;

public interface IEventTypeRepository
{
	Task<(List<EventType> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize);

	Task<EventType?> GetByIdAsync(
		Guid id,
		Guid companyId);

	Task<Guid> CreateAsync(EventType eventType);
	Task<bool> UpdateAsync(EventType eventType);

	Task<bool> DeleteAsync(
		Guid id,
		Guid companyId);

	Task<bool> ExistsInEventSchedulesAsync(
		Guid id,
		Guid companyId);

	Task<bool> UpdateSoftDeleteAsync(EventType eventType);
}