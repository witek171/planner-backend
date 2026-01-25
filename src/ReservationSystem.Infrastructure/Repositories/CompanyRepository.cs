using Microsoft.Data.SqlClient;
using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Domain.Models;
using ReservationSystem.Infrastructure.Utils;

namespace ReservationSystem.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
	private readonly string _connectionString;

	public CompanyRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<Guid> CreateAsync(Company company)
	{
		const string sql = @"
			INSERT INTO Companies 
				(Name, TaxCode, Street, City, PostalCode, Phone, Email)
			OUTPUT INSERTED.Id
			VALUES (@Name, @TaxCode, @Street, @City, @PostalCode, @Phone, @Email)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Name", company.Name);
		command.Parameters.AddWithValue("@TaxCode", company.TaxCode);
		command.Parameters.AddWithValue("@Street", company.Street);
		command.Parameters.AddWithValue("@City", company.City);
		command.Parameters.AddWithValue("@PostalCode", company.PostalCode);
		command.Parameters.AddWithValue("@Phone", company.Phone);
		command.Parameters.AddWithValue("@Email", company.Email);

		object result = (await command.ExecuteScalarAsync())!;
		return (Guid)result;
	}

	public async Task<bool> PutAsync(Company company)
	{
		const string sql = @"
			UPDATE Companies
			SET 
			Name = @Name,
			TaxCode = @TaxCode,
			Street = @Street,
			City = @City,
			PostalCode = @PostalCode,
			Phone = @Phone,
			Email = @Email
			WHERE Id = @Id";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", company.Id);
		command.Parameters.AddWithValue("@Name", company.Name);
		command.Parameters.AddWithValue("@TaxCode", company.TaxCode);
		command.Parameters.AddWithValue("@Street", company.Street);
		command.Parameters.AddWithValue("@City", company.City);
		command.Parameters.AddWithValue("@PostalCode", company.PostalCode);
		command.Parameters.AddWithValue("@Phone", company.Phone);
		command.Parameters.AddWithValue("@Email", company.Email);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<bool> DeleteByIdAsync(Guid companyId)
	{
		const string sql = @"
			DELETE FROM Companies WHERE Id = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<Company?> GetByIdAsync(Guid companyId)
	{
		const string sql = @"
			SELECT 
				Id, Name, TaxCode, Street, City, PostalCode, 
				Phone, Email, IsParentNode, IsReception, CreatedAt
			FROM Companies 
			WHERE Id = @Id";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", companyId);

		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		if (!await reader.ReadAsync())
			return null;

		return DbMapper.MapCompany(reader);
	}

	public async Task<bool> ExistsAsParentAsync(
		Guid companyId,
		SqlConnection? connection = null,
		SqlTransaction? transaction = null)
	{
		{
			bool disposeConnection = false;
			if (connection == null)
			{
				connection = new SqlConnection(_connectionString);
				await connection.OpenAsync();
				disposeConnection = true;
			}

			const string sql = @"
			SELECT CAST(
				CASE WHEN EXISTS (
					SELECT 1 FROM CompanyHierarchies
					WHERE ParentCompanyId = @ParentCompanyId
				) THEN 1 ELSE 0 END 
			AS bit)";

			await using SqlCommand command = new(sql, connection, transaction);
			command.Parameters.AddWithValue("@ParentCompanyId", companyId);

			object result = (await command.ExecuteScalarAsync())!;

			if (disposeConnection)
				await connection.DisposeAsync();
			
			return (bool)result;
		}
	}

	public async Task<bool> AddRelationAsync(
		Guid companyId,
		Guid parentCompanyId)
	{
		const string sql = @"
			INSERT INTO CompanyHierarchies (CompanyId, ParentCompanyId)
			VALUES (@CompanyId, @ParentCompanyId)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@ParentCompanyId", parentCompanyId);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<(bool isParent, Guid? parentId)> RemoveRelationsAsync(Guid companyId)
	{
		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();
		await using SqlTransaction transaction = (SqlTransaction)await connection.BeginTransactionAsync();

		try
		{
			if (await ExistsAsParentAsync(companyId, connection, transaction))
			{
				await DeleteRelationsByCompanyIdAsync(connection, transaction, companyId, includeChildren: true);
				await transaction.CommitAsync();
				return (true, null);
			}

			Guid? parentId = await GetParentCompanyIdAsync(connection, transaction, companyId);
			if (parentId != null)
				await DeleteRelationsByCompanyIdAsync(connection, transaction, companyId, includeChildren: false);

			await transaction.CommitAsync();
			return (false, parentId);
		}
		catch
		{
			await transaction.RollbackAsync();
			throw;
		}
	}

	public async Task<List<Company>> GetAllRelationsAsync(Guid companyId)
	{
		const string sql = @"
			SELECT DISTINCT c.Id, c.Name, c.TaxCode, c.Street, c.City, c.PostalCode,
			c.Phone, c.Email, c.IsParentNode, c.IsReception, c.CreatedAt
			FROM Companies c
			INNER JOIN CompanyHierarchies ch ON 
				(ch.CompanyId = c.Id AND ch.ParentCompanyId = @CompanyId) OR
				(ch.ParentCompanyId = c.Id AND ch.CompanyId = @CompanyId)
			WHERE c.Id <> @CompanyId";

		List<Company> companies = new();

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		while (await reader.ReadAsync())
			companies.Add(DbMapper.MapCompany(reader));

		return companies;
	}

	public async Task<bool> UpdateIsParentNodeFlagAsync(Company company)
	{
		const string sql = @"
			UPDATE Companies SET IsParentNode = @IsParentNode WHERE Id = @Id";
		
		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", company.Id);
		command.Parameters.AddWithValue("@IsParentNode", company.IsParentNode);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<bool> UpdateIsReceptionFlagAsync(Company company)
	{
		const string sql = @"
			UPDATE Companies SET IsReception = @IsReception WHERE Id = @Id";
		
		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", company.Id);
		command.Parameters.AddWithValue("@IsReception", company.IsReception);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<bool> RelationExistAsync(
		Guid companyId,
		Guid parentCompanyId)
	{
		const string sql = @"
			SELECT CAST(
				CASE WHEN EXISTS (
					SELECT 1 FROM CompanyHierarchies
					WHERE (CompanyId = @CompanyId AND ParentCompanyId = @ParentCompanyId)
						OR (CompanyId = @ParentCompanyId AND ParentCompanyId = @CompanyId)
				) THEN 1 ELSE 0 END 
			AS bit)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@ParentCompanyId", parentCompanyId);

		object result = (await command.ExecuteScalarAsync())!;
		return (bool)result;
	}

	public async Task<bool> ExistsAsChildAsync(Guid companyId)
	{
		const string sql = @"
			SELECT CAST(
				CASE WHEN EXISTS (
					SELECT 1 FROM CompanyHierarchies
					WHERE CompanyId = @CompanyId
				) THEN 1 ELSE 0 END 
			AS bit)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		object result = (await command.ExecuteScalarAsync())!;
		return (bool)result;
	}

	private async Task<Guid?> GetParentCompanyIdAsync(
		SqlConnection connection,
		SqlTransaction transaction,
		Guid companyId)
	{
		const string sql = @"
			SELECT ParentCompanyId
			FROM CompanyHierarchies
			WHERE CompanyId = @CompanyId";

		await using SqlCommand command = new(sql, connection, transaction);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		object? result = await command.ExecuteScalarAsync();
		return (Guid?)result;
	}

	private async Task DeleteRelationsByCompanyIdAsync(
		SqlConnection connection,
		SqlTransaction transaction,
		Guid companyId,
		bool includeChildren)
	{
		string sql = includeChildren
			? @"DELETE FROM CompanyHierarchies
			WHERE ParentCompanyId = @CompanyId
				OR CompanyId = @CompanyId"
			: @"DELETE FROM CompanyHierarchies
			WHERE CompanyId = @CompanyId";

		await using SqlCommand command = new(sql, connection, transaction);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		await command.ExecuteNonQueryAsync();
	}
}