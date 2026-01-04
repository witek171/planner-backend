using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class CompanyConfigService : ICompanyConfigService
{
	private readonly ICompanyConfigRepository _companyConfigRepository;

	public CompanyConfigService(ICompanyConfigRepository companyConfigRepository)
	{
		_companyConfigRepository = companyConfigRepository;
	}

	public async Task UpdateBreakTimesAsync(CompanyConfig companyConfig)
	{
		if (companyConfig.BreakTimeStaff < 0)
			throw new InvalidOperationException(
				"Break time for staff must be equal or greater than zero");

		if (companyConfig.BreakTimeParticipants < 0)
			throw new InvalidOperationException(
				"Break time for participants must be equal or greater than zero");

		await _companyConfigRepository.UpdateBreakTimesAsync(companyConfig);
	}

	public async Task<CompanyConfig?> GetByIdAsync(Guid companyId)
		=> await _companyConfigRepository.GetByIdAsync(companyId);
}