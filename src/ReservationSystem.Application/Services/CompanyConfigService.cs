using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Domain.Exceptions;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Services;

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