using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Domain.Exceptions;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class StaffMemberSpecializationService : IStaffMemberSpecializationService
{
	private readonly IStaffMemberSpecializationRepository _staffMemberSpecializationRepository;

	public StaffMemberSpecializationService(IStaffMemberSpecializationRepository staffMemberSpecializationRepository)
	{
		_staffMemberSpecializationRepository = staffMemberSpecializationRepository;
	}

	public async Task<Guid> CreateAsync(
		Guid companyId,
		StaffMemberSpecialization staffMemberSpecialization)
	{
		Guid staffMemberId = staffMemberSpecialization.StaffMemberId;
		Guid specializationId = staffMemberSpecialization.SpecializationId;
		if (await _staffMemberSpecializationRepository.ExistsAsync(staffMemberId, specializationId))
			throw new StaffMemberSpecializationAlreadyAssignedException(staffMemberId, specializationId);

		return await _staffMemberSpecializationRepository.CreateAsync(companyId, staffMemberSpecialization);
	}

	public async Task DeleteAsync(
		Guid id,
		Guid companyId)
		=> await _staffMemberSpecializationRepository.DeleteByIdAsync(id, companyId);

	public async Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id)
		=> await _staffMemberSpecializationRepository.ExistsByIdAsync(companyId, id);
}