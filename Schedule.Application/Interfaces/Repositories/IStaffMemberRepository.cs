using Schedule.Domain.Models;

namespace Schedule.Application.Interfaces.Repositories;

public interface IStaffMemberRepository
{
	Task<(List<StaffMember> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize);

	Task<StaffMember?> GetByIdAsync(
		Guid staffMemberId,
		Guid companyId);

	Task<StaffMember?> GetByEmailAsync(string email);

	Task<Guid> CreateAsync(StaffMember staffMember);
	Task<bool> PutAsync(StaffMember staffMember);

	Task<bool> DeleteByIdAsync(
		Guid companyId,
		Guid staffMemberId);

	Task<bool> HasRelatedRecordsAsync(
		Guid staffMemberId,
		Guid companyId);

	Task<bool> EmailExistsForOtherAsync(
		Guid companyId,
		Guid staffMemberId,
		string email);

	Task<bool> PhoneExistsForOtherAsync(
		Guid companyId,
		Guid staffMemberId,
		string phone);

	Task<bool> EmailExistsForOtherWithoutCompanyIdAsync(
		Guid staffMemberId,
		string email);

	Task<bool> PhoneExistsForOtherWithoutCompanyIdAsync(
		Guid staffMemberId,
		string phone);

	Task<bool> UpdateSoftDeleteAsync(StaffMember staffMember);

	Task<Guid> AssignToCompanyAsync(Guid staffMemberId, Guid companyId);
	Task<bool> UnassignFromCompanyAsync(Guid staffMemberId, Guid companyId);

	Task<List<StaffMemberCompany>> GetAssignedCompanyAsync(Guid staffMemberId);
}