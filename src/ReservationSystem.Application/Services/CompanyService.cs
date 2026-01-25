using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Application.Interfaces.Services;
using ReservationSystem.Domain.Exceptions;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Services;

public class CompanyService : ICompanyService
{
	private readonly ICompanyRepository _companyRepository;
	private readonly ICompanyConfigRepository _companyConfigRepository;

	public CompanyService(ICompanyRepository companyRepository,
		ICompanyConfigRepository companyConfigRepository)
	{
		_companyRepository = companyRepository;
		_companyConfigRepository = companyConfigRepository;
	}

	public async Task<Guid> CreateAsync(Company company)
	{
		company.Normalize();
		Guid companyId = await _companyRepository.CreateAsync(company);
		await _companyConfigRepository.CreateAsync(companyId);

		return companyId;
	}

	public async Task PutAsync(Company company)
	{
		company.Normalize();
		await _companyRepository.PutAsync(company);
	}

	public async Task DeleteByIdAsync(Guid companyId)
		=> await _companyRepository.DeleteByIdAsync(companyId);

	public async Task<Company?> GetByIdAsync(Guid companyId)
		=> await _companyRepository.GetByIdAsync(companyId);

	public async Task MarkAsReceptionAsync(Company company)
	{
		company.MarkAsReception();
		await _companyRepository.UpdateIsReceptionFlagAsync(company);
	}

	public async Task UnmarkAsReceptionAsync(Company company)
	{
		company.UnmarkAsReception();
		await _companyRepository.UpdateIsReceptionFlagAsync(company);
	}

	public async Task AddRelationAsync(
		Guid childId,
		Guid parentId)
	{
		if (childId == parentId)
			throw new CompanySelfReferenceException(childId);

		if (await _companyRepository.ExistsAsChildAsync(childId))
			throw new CompanyAlreadyHasParentException(childId);

		if (await _companyRepository.RelationExistAsync(childId, parentId))
			throw new CompanyRelationAlreadyExistsException(childId, parentId);

		await _companyRepository.AddRelationAsync(childId, parentId);
		Company parent = (await _companyRepository.GetByIdAsync(parentId))!;

		if (!parent.IsParentNode)
		{
			parent.MarkAsParentNode();
			await _companyRepository.UpdateIsParentNodeFlagAsync(parent);
		}
	}

	public async Task RemoveRelationsAsync(Guid companyId)
	{
		if (!await _companyRepository.ExistsAsChildAsync(companyId) &&
			!await _companyRepository.ExistsAsParentAsync(companyId))
			throw new CompanyNotInHierarchyException(companyId);

		(bool hasChildren, Guid? parentId) = await _companyRepository
			.RemoveRelationsAsync(companyId);

		if (!hasChildren && parentId is Guid id)
			await UnmarkCompanyAsParentIfNeededAsync(id);

		await UnmarkCompanyAsParentIfNeededAsync(companyId);
	}

	public async Task<List<Company>> GetAllRelationsAsync(Guid companyId)
		=> await _companyRepository.GetAllRelationsAsync(companyId);

	private async Task UnmarkCompanyAsParentIfNeededAsync(Guid companyId)
	{
		Company company = (await _companyRepository.GetByIdAsync(companyId))!;
		if (!await _companyRepository.ExistsAsParentAsync(companyId) && company.IsParentNode)
		{
			company.UnmarkAsParentNode();
			await _companyRepository.UpdateIsParentNodeFlagAsync(company);
		}
	}
}