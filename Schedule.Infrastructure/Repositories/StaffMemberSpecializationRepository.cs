using Microsoft.Data.SqlClient;
using Schedule.Application.Interfaces.Repositories;
using Schedule.Domain.Models;

namespace Schedule.Infrastructure.Repositories;

public class StaffMemberSpecializationRepository : IStaffMemberSpecializationRepository
{
	private readonly string _connectionString;

	public StaffMemberSpecializationRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<Guid> CreateAsync(
		Guid companyId,
		StaffMemberSpecialization staffMemberSpecialization)
	{
		const string sql = @"
			INSERT INTO StaffMemberSpecializations (CompanyId, StaffMemberId, SpecializationId)
			OUTPUT INSERTED.Id
			VALUES (@CompanyId, @StaffMemberId, @SpecializationId)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberSpecialization.StaffMemberId);
		command.Parameters.AddWithValue("@SpecializationId", staffMemberSpecialization.SpecializationId);

		object result = (await command.ExecuteScalarAsync())!;
		return (Guid)result;
	}

	public async Task<bool> DeleteByIdAsync(
		Guid companyId,
		Guid staffMemberSpecializationId)
	{
		const string sql = @"
			DELETE FROM StaffMemberSpecializations 
			WHERE CompanyId = @CompanyId AND Id = @Id";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@Id", staffMemberSpecializationId);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<bool> ExistsAsync(
		Guid staffMemberId,
		Guid specializationId)
	{
		const string sql = @"
			SELECT 1
			FROM StaffMemberSpecializations
			WHERE StaffMemberId = @StaffMemberId
			AND SpecializationId = @SpecializationId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);
		command.Parameters.AddWithValue("@SpecializationId", specializationId);

		object? result = await command.ExecuteScalarAsync();

		return result != null;
	}

	public async Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id)
	{
		const string sql = @"
			SELECT 1
			FROM StaffMemberSpecializations
			WHERE CompanyId = @CompanyId AND Id = @Id";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@Id", id);

		object? result = await command.ExecuteScalarAsync();
		return result != null;
	}
}