using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Services;

public interface IStaffMemberAvailabilityService
{
	Task<List<StaffMemberAvailability>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId);

	Task<Guid> CreateAsync(StaffMemberAvailability availability);

	Task DeleteAsync(
		Guid companyId,
		Guid id);

	Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id);
}