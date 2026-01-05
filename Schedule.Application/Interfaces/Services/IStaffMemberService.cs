using Schedule.Application.ReadModels;
using Schedule.Domain.Models;

namespace Schedule.Application.Interfaces.Services;

public interface IStaffMemberService
{
	Task<(List<StaffMember> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize);

	Task<StaffMember?> GetByIdAsync(
		Guid id,
		Guid companyId);

	Task<StaffMember?> GetByEmailAsync(String email);

	Task<Guid> CreateAsync(
		StaffMember staffMember,
		Guid companyId);

	Task PutAsync(StaffMember staffMember, Guid companyId);

	Task DeleteAsync(
		Guid id,
		Guid companyId);

	Task<Guid> AssignToCompanyAsync(Guid staffMemberId, Guid companyId);
	Task<bool> UnassignFromCompanyAsync(Guid staffMemberId, Guid companyId);
	Task<StaffMemberCompanies> GetAssignedCompanyAsync(Guid staffMemberId);
}