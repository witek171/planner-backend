using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Services;

public interface IEventScheduleStaffMemberService
{
	Task<List<EventScheduleStaffMember>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid eventId);

	Task<Guid> CreateAsync(EventScheduleStaffMember entity);

	Task DeleteAsync(
		Guid companyId,
		Guid id);

	Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id);
}