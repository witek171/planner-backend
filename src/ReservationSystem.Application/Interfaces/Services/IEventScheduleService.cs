using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Services;

public interface IEventScheduleService
{
	Task<List<EventSchedule>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId);

	Task<(List<EventSchedule> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize,
		Guid? eventTypeId);

	Task<EventSchedule?> GetByIdAsync(
		Guid id,
		Guid companyId);

	Task<Guid> CreateAsync(EventSchedule eventSchedule);
	Task UpdateAsync(EventSchedule eventSchedule);

	Task DeleteAsync(
		Guid id,
		Guid companyId);
}