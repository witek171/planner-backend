using Microsoft.Data.SqlClient;
using Schedule.Application.Interfaces.Repositories;
using Schedule.Domain.Models;
using Schedule.Infrastructure.Utils;

namespace Schedule.Infrastructure.Repositories;

public class SpecializationRepository : ISpecializationRepository
{
	private readonly string _connectionString;

	public SpecializationRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<(List<Specialization> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize,
		string? search = null)
	{
		string sql = @"
			SELECT Id, CompanyId, Name, Description,
				COUNT(*) OVER() AS TotalCount
			FROM Specializations 
			WHERE CompanyId = @CompanyId";

		if (!string.IsNullOrWhiteSpace(search))
			sql += " AND (Name LIKE @Search OR Description LIKE @Search)";

		sql += @"
			ORDER BY Name
			OFFSET @Offset ROWS
			FETCH NEXT @PageSize ROWS ONLY";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
		command.Parameters.AddWithValue("@PageSize", pageSize);
		command.Parameters.AddWithValue("@Search",
			string.IsNullOrWhiteSpace(search)
				? DBNull.Value
				: $"%{search}%");

		await using SqlDataReader reader = await command.ExecuteReaderAsync();
		List<Specialization> specializations = new();
		int totalCount = 0;
		while (await reader.ReadAsync())
		{
			if (totalCount == 0)
				totalCount = Convert.ToInt32(reader["TotalCount"]);

			specializations.Add(DbMapper.MapSpecialization(reader));
		}

		return (specializations, totalCount);
	}

	public async Task<Specialization?> GetByIdAsync(Guid id, Guid companyId)
	{
		const string sql = @"
			SELECT Id, CompanyId, Name, Description 
			FROM Specializations 
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();
		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		SqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
			return DbMapper.MapSpecialization(reader);

		return null;
	}

	public async Task<Guid> CreateAsync(Specialization specialization)
	{
		const string sql = @"
			INSERT INTO Specializations (CompanyId, Name, Description)
			OUTPUT INSERTED.Id
			VALUES (@CompanyId, @Name, @Description)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", specialization.CompanyId);
		command.Parameters.AddWithValue("@Name", specialization.Name);
		command.Parameters.AddWithValue("@Description", specialization.Description);

		object result = (await command.ExecuteScalarAsync())!;
		return (Guid)result;
	}

	public async Task<bool> UpdateAsync(Specialization specialization)
	{
		const string sql = @"
			UPDATE Specializations 
			SET Name = @Name, Description = @Description 
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();
		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", specialization.Id);
		command.Parameters.AddWithValue("@CompanyId", specialization.CompanyId);
		command.Parameters.AddWithValue("@Name", specialization.Name);
		command.Parameters.AddWithValue("@Description", specialization.Description);
		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}

	public async Task<bool> DeleteAsync(Guid id, Guid companyId)
	{
		const string sql = @"
			DELETE FROM Specializations WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();
		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}
}