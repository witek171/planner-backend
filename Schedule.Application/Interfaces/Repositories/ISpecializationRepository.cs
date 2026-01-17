using Schedule.Domain.Models;

namespace Schedule.Application.Interfaces.Repositories;

public interface ISpecializationRepository
{
	Task<(List<Specialization> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize,
		string? search);

	Task<Specialization?> GetByIdAsync(Guid id, Guid companyId);
	Task<Guid> CreateAsync(Specialization specialization);
	Task<bool> UpdateAsync(Specialization specialization);
	Task<bool> DeleteAsync(Guid id, Guid companyId);
}