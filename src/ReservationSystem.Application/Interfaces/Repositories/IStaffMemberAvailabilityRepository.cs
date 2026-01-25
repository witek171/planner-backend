using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Repositories;

public interface IStaffMemberAvailabilityRepository
{
	Task<List<StaffMemberAvailability>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId);

	Task<Guid> CreateAsync(StaffMemberAvailability availability);

	Task<bool> DeleteByIdAsync(
		Guid staffMemberAvailabilityId,
		Guid companyId);

	Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id);
}