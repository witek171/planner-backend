using ReservationSystem.Contracts.Dtos.Responses;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Services;

public interface IEventTypeService
{
	Task<(List<EventType> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize);

	Task<EventType?> GetByIdAsync(
		Guid id,
		Guid companyId);

	Task<Guid> CreateAsync(EventType eventType);
	Task UpdateAsync(EventType eventType);

	Task DeleteAsync(
		Guid id,
		Guid companyId);
}