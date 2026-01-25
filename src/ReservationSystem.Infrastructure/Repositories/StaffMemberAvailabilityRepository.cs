using Microsoft.Data.SqlClient;
using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Domain.Models;
using ReservationSystem.Infrastructure.Utils;

namespace ReservationSystem.Infrastructure.Repositories;

public class StaffMemberAvailabilityRepository : IStaffMemberAvailabilityRepository
{
	private readonly string _connectionString;

	public StaffMemberAvailabilityRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<Guid> CreateAsync(StaffMemberAvailability availability)
	{
		const string sql = @"
			INSERT INTO StaffMemberAvailabilities 
			(CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
			OUTPUT INSERTED.Id
			VALUES (@CompanyId, @StaffMemberId, @Date, @StartTime, @EndTime, @IsAvailable)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", availability.CompanyId);
		command.Parameters.AddWithValue("@StaffMemberId", availability.StaffMemberId);
		command.Parameters.AddWithValue("@Date", availability.Date.ToDateTime(TimeOnly.MinValue));
		command.Parameters.AddWithValue("@StartTime", availability.StartTime);
		command.Parameters.AddWithValue("@EndTime", availability.EndTime);
		command.Parameters.AddWithValue("@IsAvailable", availability.IsAvailable);

		object result = (await command.ExecuteScalarAsync())!;
		return (Guid)result;
	}

	public async Task<bool> DeleteByIdAsync(
		Guid companyId,
		Guid staffMemberAvailabilityId)
	{
		const string sql = @"
			DELETE FROM StaffMemberAvailabilities 
			WHERE CompanyId = @CompanyId AND Id = @Id";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@Id", staffMemberAvailabilityId);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<List<StaffMemberAvailability>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId)
	{
		const string sql = @"
			SELECT Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable
			FROM StaffMemberAvailabilities
			WHERE StaffMemberId = @StaffMemberId AND CompanyId = @CompanyId
			AND IsAvailable = 1";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		List<StaffMemberAvailability> availabilities = new();

		while (await reader.ReadAsync())
			availabilities.Add(DbMapper.MapStaffMemberAvailability(reader));

		return availabilities;
	}

	public async Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id)
	{
		const string sql = @"
			SELECT 1
			FROM StaffMemberAvailabilities
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