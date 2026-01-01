using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Application.Interfaces.Utils;
using Schedule.Domain.Exceptions;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class StaffMemberService : IStaffMemberService
{
	private readonly IStaffMemberRepository _staffMemberRepository;
	private readonly IPasswordHasher _passwordHasher;

	public StaffMemberService(
		IStaffMemberRepository staffMemberRepository,
		IPasswordHasher passwordHasher)
	{
		_staffMemberRepository = staffMemberRepository;
		_passwordHasher = passwordHasher;
	}

	public async Task<(List<StaffMember> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize)
		=> await _staffMemberRepository.GetPagedWithCountAsync(companyId, page, pageSize);

	public async Task<StaffMember?> GetByIdAsync(
		Guid id,
		Guid companyId)
		=> await _staffMemberRepository.GetByIdAsync(id, companyId);

	public async Task<StaffMember?> GetByEmailAsync(String email)
		=> await _staffMemberRepository.GetByEmailAsync(email);

	public async Task<Guid> CreateAsync(
		StaffMember staffMember,
		Guid companyId)
	{
		staffMember.Normalize();
		await ValidateEmailAndPhoneAsync(staffMember, companyId);
		string hashed = _passwordHasher.Hash(staffMember.Password);
		staffMember.SetPassword(hashed);

		Guid staffMemberId = await _staffMemberRepository.CreateAsync(staffMember);
		await _staffMemberRepository.AssignToCompanyAsync(staffMemberId, companyId);
		return staffMemberId;
	}

	public async Task PutAsync(
		StaffMember staffMember,
		Guid companyId)
	{
		// trzeba by sprawdzic czy tel i email nie koliduje z firmami pracownika
		staffMember.Normalize();
		await ValidateEmailAndPhoneAsync(staffMember, companyId);
		await _staffMemberRepository.PutAsync(staffMember);
	}

	public async Task DeleteAsync(
		Guid id,
		Guid companyId)
	{
		if (await _staffMemberRepository.HasRelatedRecordsAsync(id, companyId))
		{
			StaffMember staffMember = (await _staffMemberRepository.GetByIdAsync(id, companyId))!;
			staffMember.SoftDelete();
			await _staffMemberRepository.UpdateSoftDeleteAsync(staffMember);
		}
		else
			await _staffMemberRepository.DeleteByIdAsync(id, companyId);
		// moze usuniecie rekordu/rekordow z staffCompanies
	}

	public async Task<Guid> AssignToCompanyAsync(
		Guid staffMemberId,
		Guid companyId)
	{
		// trzeba by sprawdzic czy tel i email nie koliduje z firmami pracownika i inne
		return await _staffMemberRepository.AssignToCompanyAsync(staffMemberId, companyId);
	}

	public async Task<bool> UnassignFromCompanyAsync(
		Guid staffMemberId,
		Guid companyId)
	{
		return await _staffMemberRepository.UnassignFromCompanyAsync(staffMemberId, companyId);
	}

	public async Task<List<StaffMemberCompany>> GetAssignedCompanyAsync(Guid staffMemberId)
	{
		return await _staffMemberRepository.GetAssignedCompanyAsync(staffMemberId);
	}

	private async Task ValidateEmailAndPhoneAsync(
		StaffMember staffMember,
		Guid companyId)
	{
		Guid staffMemberId = staffMember.Id;
		string email = staffMember.Email;
		string phone = staffMember.Phone;

		if (await _staffMemberRepository.EmailExistsForOtherAsync(companyId, staffMemberId, email))
			throw new EmailAlreadyExistsException(email, companyId);

		if (await _staffMemberRepository.PhoneExistsForOtherAsync(companyId, staffMemberId, phone))
			throw new PhoneAlreadyExistsException(phone, companyId);
	}

	// private async Task ValidateEmailAndPhoneWithoutCompanyIdAsync(
	// 	StaffMember staffMember)
	// {
	// 	Guid staffMemberId = staffMember.Id;
	// 	string email = staffMember.Email;
	// 	string phone = staffMember.Phone;
	//
	// 	if (await _staffMemberRepository.EmailExistsForOtherWithoutCompanyIdAsync(staffMemberId, email))
	// 		throw new EmailAlreadyExistsException(email);
	//
	// 	if (await _staffMemberRepository.PhoneExistsForOtherWithoutCompanyIdAsync(staffMemberId, phone))
	// 		throw new PhoneAlreadyExistsException(phone);
	// }
}