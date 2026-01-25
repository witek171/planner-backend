using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Services;

public interface IStaffMemberSpecializationService
{
	Task<Guid> CreateAsync(
		Guid companyId,
		StaffMemberSpecialization staffMemberSpecialization);

	Task DeleteAsync(
		Guid id,
		Guid companyId);

	Task<bool> ExistsByIdAsync(
		Guid id,
		Guid companyId);
}