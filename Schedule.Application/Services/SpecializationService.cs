using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class SpecializationService : ISpecializationService
{
	private readonly ISpecializationRepository _specializationRepository;

	public SpecializationService(ISpecializationRepository specializationRepository)
	{
		_specializationRepository = specializationRepository;
	}

	public async Task<(List<Specialization> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize)
		=> await _specializationRepository.GetPagedWithCountAsync(companyId, page, pageSize);

	public async Task<Specialization?> GetByIdAsync(Guid id, Guid companyId)
		=> await _specializationRepository.GetByIdAsync(id, companyId);

	public async Task<Guid> CreateAsync(Specialization specialization)
		=> await _specializationRepository.CreateAsync(specialization);

	public async Task<bool> UpdateAsync(Specialization specialization)
		=> await _specializationRepository.UpdateAsync(specialization);

	public async Task<bool> DeleteAsync(Guid id, Guid companyId)
		=> await _specializationRepository.DeleteAsync(id, companyId);
}