using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Repositories;

public interface ICompanyConfigRepository
{
	Task CreateAsync(Guid companyId);
	Task<bool> UpdateBreakTimesAsync(CompanyConfig companyConfig);
	Task<CompanyConfig?> GetByIdAsync(Guid companyId);
}