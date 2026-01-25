using Microsoft.Data.SqlClient;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Repositories;

public interface ICompanyRepository
{
	Task<Guid> CreateAsync(Company company);
	Task<bool> PutAsync(Company company);
	Task<bool> DeleteByIdAsync(Guid companyId);
	Task<Company?> GetByIdAsync(Guid companyId);

	Task<bool> ExistsAsParentAsync(
		Guid companyId,
		SqlConnection? connection = null,
		SqlTransaction? transaction = null);

	Task<bool> AddRelationAsync(
		Guid childId,
		Guid parentId);

	Task<(bool isParent, Guid? parentId)> RemoveRelationsAsync(Guid companyId);
	Task<List<Company>> GetAllRelationsAsync(Guid companyId);
	Task<bool> UpdateIsParentNodeFlagAsync(Company company);
	Task<bool> UpdateIsReceptionFlagAsync(Company company);

	Task<bool> RelationExistAsync(
		Guid companyId,
		Guid parentCompanyId);

	Task<bool> ExistsAsChildAsync(Guid companyId);
}