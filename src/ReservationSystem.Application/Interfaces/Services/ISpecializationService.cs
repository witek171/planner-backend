using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Services;

public interface ISpecializationService
{
	Task<(List<Specialization> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize,
		string? search);

	Task<Specialization?> GetByIdAsync(Guid id, Guid companyId);
	Task<Guid> CreateAsync(Specialization specialization);
	Task<bool> UpdateAsync(Specialization specialization);
	Task<bool> DeleteAsync(Guid id, Guid companyId);
}