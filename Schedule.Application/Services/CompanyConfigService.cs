using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Domain.Exceptions;
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
			throw new InvalidBreakTimeStaffException();

		if (companyConfig.BreakTimeParticipants < 0)
			throw new InvalidBreakTimeParticipantsException();

		await _companyConfigRepository.UpdateBreakTimesAsync(companyConfig);
	}

	public async Task<CompanyConfig?> GetByIdAsync(Guid companyId)
		=> await _companyConfigRepository.GetByIdAsync(companyId);
}