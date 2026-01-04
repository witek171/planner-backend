using Schedule.Domain.Models;

namespace Schedule.Application.Interfaces.Services;

public interface ICompanyConfigService
{
	Task UpdateBreakTimesAsync(CompanyConfig companyConfig);
	Task<CompanyConfig?> GetByIdAsync(Guid companyId);
}