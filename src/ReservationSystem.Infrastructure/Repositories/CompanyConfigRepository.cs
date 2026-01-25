using Microsoft.Data.SqlClient;
using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Domain.Models;
using ReservationSystem.Infrastructure.Utils;

namespace ReservationSystem.Infrastructure.Repositories;

public class CompanyConfigRepository : ICompanyConfigRepository
{
	private readonly string _connectionString;

	public CompanyConfigRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task CreateAsync(Guid companyId)
	{
		const string sql = @"
			INSERT INTO CompanyConfigs (CompanyId)
			VALUES (@CompanyId)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		await command.ExecuteNonQueryAsync();
	}

	public async Task<bool> UpdateBreakTimesAsync(CompanyConfig companyConfig)
	{
		const string sql = @"
			UPDATE CompanyConfigs
			SET BreakTimeStaff = @BreakTimeStaff, BreakTimeParticipants = @BreakTimeParticipants
			WHERE CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyConfig.CompanyId);
		command.Parameters.AddWithValue("@BreakTimeStaff", companyConfig.BreakTimeStaff);
		command.Parameters.AddWithValue("@BreakTimeParticipants", companyConfig.BreakTimeParticipants);

		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}

	public async Task<CompanyConfig?> GetByIdAsync(Guid companyId)
	{
		const string sql = @"
			SELECT CompanyId, BreakTimeStaff, BreakTimeParticipants
			FROM CompanyConfigs
			WHERE CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		await using SqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
			return DbMapper.MapCompanyConfig(reader);

		return null;
	}
}