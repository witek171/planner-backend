using Microsoft.Data.SqlClient;
using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Domain.Models;
using ReservationSystem.Infrastructure.Utils;

namespace ReservationSystem.Infrastructure.Repositories;

public class EventTypeRepository : IEventTypeRepository
{
	private readonly string _connectionString;

	public EventTypeRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<(List<EventType> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize)
	{
		const string sql = @"
			SELECT Id, CompanyId, Name, Description, Duration, 
				Price, MaxParticipants, MinStaff, IsDeleted,
				COUNT(*) OVER() AS TotalCount
			FROM EventTypes 
			WHERE CompanyId = @CompanyId AND IsDeleted = 0
			ORDER BY Name
			OFFSET @Offset ROWS
			FETCH NEXT @PageSize ROWS ONLY";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
		command.Parameters.AddWithValue("@PageSize", pageSize);

		await using SqlDataReader reader = await command.ExecuteReaderAsync();
		List<EventType> eventTypes = new();
		int totalCount = 0;
		while (await reader.ReadAsync())
		{
			if (totalCount == 0)
				totalCount = Convert.ToInt32(reader["TotalCount"]);

			eventTypes.Add(DbMapper.MapEventType(reader));
		}

		return (eventTypes, totalCount);
	}

	public async Task<EventType?> GetByIdAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			SELECT Id, CompanyId, Name, Description, Duration, 
			Price, MaxParticipants, MinStaff, isDeleted
			FROM EventTypes 
			WHERE Id = @Id AND CompanyId = @CompanyId AND isDeleted = 0";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		SqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
			return DbMapper.MapEventType(reader);

		return null;
	}

	public async Task<Guid> CreateAsync(EventType eventType)
	{
		const string sql = @"
			INSERT INTO EventTypes 
			(CompanyId, Name, Description, Duration, Price, MaxParticipants, MinStaff)
			OUTPUT INSERTED.Id
			VALUES 
			(@CompanyId, @Name, @Description, @Duration, @Price, @MaxParticipants, @MinStaff)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", eventType.CompanyId);
		command.Parameters.AddWithValue("@Name", eventType.Name);
		command.Parameters.AddWithValue("@Description", eventType.Description);
		command.Parameters.AddWithValue("@Duration", eventType.Duration);
		command.Parameters.AddWithValue("@Price", eventType.Price);
		command.Parameters.AddWithValue("@MaxParticipants", eventType.MaxParticipants);
		command.Parameters.AddWithValue("@MinStaff", eventType.MinStaff);

		object result = (await command.ExecuteScalarAsync())!;
		return (Guid)result;
	}

	public async Task<bool> UpdateAsync(EventType eventType)
	{
		const string sql = @"
			UPDATE EventTypes 
			SET Name = @Name, Description = @Description, Duration = @Duration,
				Price = @Price, MaxParticipants = @MaxParticipants, MinStaff = @MinStaff
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", eventType.Id);
		command.Parameters.AddWithValue("@CompanyId", eventType.CompanyId);
		command.Parameters.AddWithValue("@Name", eventType.Name);
		command.Parameters.AddWithValue("@Description", eventType.Description);
		command.Parameters.AddWithValue("@Duration", eventType.Duration);
		command.Parameters.AddWithValue("@Price", eventType.Price);
		command.Parameters.AddWithValue("@MaxParticipants", eventType.MaxParticipants);
		command.Parameters.AddWithValue("@MinStaff", eventType.MinStaff);

		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}

	public async Task<bool> DeleteAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			DELETE FROM EventTypes WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}

	public async Task<bool> ExistsInEventSchedulesAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			SELECT CASE WHEN EXISTS (
				SELECT 1 FROM EventSchedules 
				WHERE EventTypeId = @EventTypeId AND CompanyId = @CompanyId
			) THEN 1 ELSE 0 END";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@EventTypeId", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		object result = (await command.ExecuteScalarAsync())!;
		return (int)result == 1;
	}

	public async Task<bool> UpdateSoftDeleteAsync(EventType eventType)
	{
		const string sql = @"
			UPDATE EventTypes SET IsDeleted = @IsDeleted
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", eventType.Id);
		command.Parameters.AddWithValue("@CompanyId", eventType.CompanyId);
		command.Parameters.AddWithValue("@IsDeleted", eventType.IsDeleted);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}
}