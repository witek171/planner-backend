using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Repositories;

public interface IStaffMemberSpecializationRepository
{
	Task<Guid> CreateAsync(
		Guid companyId,
		StaffMemberSpecialization specialization);

	Task<bool> DeleteByIdAsync(
		Guid companyId,
		Guid id);

	Task<bool> ExistsAsync(
		Guid staffMemberId,
		Guid specializationId);

	Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id);
}