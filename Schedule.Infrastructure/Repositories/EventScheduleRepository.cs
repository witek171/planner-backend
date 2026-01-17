using Microsoft.Data.SqlClient;
using Schedule.Application.Interfaces.Repositories;
using Schedule.Domain.Models;
using Schedule.Domain.Models.Enums;
using Schedule.Infrastructure.Utils;

namespace Schedule.Infrastructure.Repositories;

public class EventScheduleRepository : IEventScheduleRepository
{
	private readonly string _connectionString;

	public EventScheduleRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<List<EventSchedule>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId)
	{
		const string sql = @"
			SELECT 
				es.Id, es.CompanyId, es.EventTypeId, es.PlaceName, 
				es.StartTime, es.CreatedAt, es.Status,
				et.Name as EventTypeName, 
				et.Description as EventTypeDescription, 
				et.Duration, et.Price, et.MaxParticipants, et.MinStaff,
				et.IsDeleted as EventTypeIsDeleted
			FROM EventSchedules es
			INNER JOIN EventScheduleStaff ess ON es.Id = ess.EventScheduleId
			INNER JOIN EventTypes et ON es.EventTypeId = et.Id
			WHERE es.CompanyId = @CompanyId AND es.Status <> @DeletedStatus 
			AND ess.StaffMemberId = @StaffMemberId
			ORDER BY es.StartTime";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);
		command.Parameters.AddWithValue("@DeletedStatus", nameof(EventScheduleStatus.Deleted));

		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		List<EventSchedule> eventSchedules = new();
		while (await reader.ReadAsync())
			eventSchedules.Add(DbMapper.MapEventSchedule(reader));

		return eventSchedules;
	}

	public async Task<(List<EventSchedule> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize,
		Guid? eventTypeId = null)
	{
		string sql = @"
			SELECT 
				es.Id, es.CompanyId, es.EventTypeId, es.PlaceName, 
				es.StartTime, es.CreatedAt, es.Status,
				et.Name as EventTypeName, 
				et.Description as EventTypeDescription, 
				et.Duration, et.Price, et.MaxParticipants, et.MinStaff,
				et.IsDeleted as EventTypeIsDeleted,
				COUNT(*) OVER() AS TotalCount
			FROM EventSchedules es
			INNER JOIN EventTypes et ON es.EventTypeId = et.Id
			WHERE es.CompanyId = @CompanyId 
			  AND es.Status <> @DeletedStatus";

		if (eventTypeId.HasValue)
			sql += " AND es.EventTypeId = @EventTypeId";

		sql += @"
			ORDER BY es.StartTime
			OFFSET @Offset ROWS
			FETCH NEXT @PageSize ROWS ONLY";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@DeletedStatus", nameof(EventScheduleStatus.Deleted));
		command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
		command.Parameters.AddWithValue("@PageSize", pageSize);
		if (eventTypeId.HasValue)
			command.Parameters.AddWithValue("@EventTypeId", eventTypeId.Value);

		await using SqlDataReader reader = await command.ExecuteReaderAsync();
		List<EventSchedule> eventSchedules = [];
		int totalCount = 0;
		while (await reader.ReadAsync())
		{
			if (totalCount == 0)
				totalCount = Convert.ToInt32(reader["TotalCount"]);

			eventSchedules.Add(DbMapper.MapEventSchedule(reader));
		}

		return (eventSchedules, totalCount);
	}

	public async Task<EventSchedule?> GetByIdAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			SELECT 
				es.Id, es.CompanyId, es.EventTypeId, es.PlaceName, 
				es.StartTime, es.CreatedAt, es.Status,
				et.Name as EventTypeName, 
				et.Description as EventTypeDescription, 
				et.Duration, et.Price, et.MaxParticipants, et.MinStaff, 
				et.IsDeleted as EventTypeIsDeleted
			FROM EventSchedules es
			INNER JOIN EventTypes et ON es.EventTypeId = et.Id
			WHERE es.Id = @Id AND es.CompanyId = @CompanyId 
			AND es.Status <> @DeletedStatus";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@DeletedStatus", nameof(EventScheduleStatus.Deleted));

		SqlDataReader reader = await command.ExecuteReaderAsync();
		if (await reader.ReadAsync())
			return DbMapper.MapEventSchedule(reader);

		return null;
	}

	public async Task<Guid> CreateAsync(EventSchedule eventSchedule)
	{
		const string sql = @"
			INSERT INTO EventSchedules 
			(CompanyId, EventTypeId, PlaceName, StartTime)
			OUTPUT INSERTED.Id
			VALUES 
			(@CompanyId, @EventTypeId, @PlaceName, @StartTime)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", eventSchedule.CompanyId);
		command.Parameters.AddWithValue("@EventTypeId", eventSchedule.EventTypeId);
		command.Parameters.AddWithValue("@PlaceName", eventSchedule.PlaceName);
		command.Parameters.AddWithValue("@StartTime", eventSchedule.StartTime);

		object result = (await command.ExecuteScalarAsync())!;
		return (Guid)result;
	}

	public async Task<bool> UpdateAsync(EventSchedule eventSchedule)
	{
		const string sql = @"
			UPDATE EventSchedules 
			SET EventTypeId = @EventTypeId, PlaceName = @PlaceName, StartTime = @StartTime
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", eventSchedule.Id);
		command.Parameters.AddWithValue("@CompanyId", eventSchedule.CompanyId);
		command.Parameters.AddWithValue("@EventTypeId", eventSchedule.EventTypeId);
		command.Parameters.AddWithValue("@PlaceName", eventSchedule.PlaceName);
		command.Parameters.AddWithValue("@StartTime", eventSchedule.StartTime);

		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}

	public async Task<bool> DeleteAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			DELETE FROM EventSchedules WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}

	public async Task<bool> HasRelatedRecordsAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			SELECT CASE WHEN EXISTS (
				SELECT 1 FROM EventScheduleStaff 
				WHERE EventScheduleId = @EventScheduleId AND CompanyId = @CompanyId
				UNION ALL
				SELECT 1 FROM Reservations 
				WHERE EventScheduleId = @EventScheduleId AND CompanyId = @CompanyId
			) THEN 1 ELSE 0 END";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@EventScheduleId", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		object result = (await command.ExecuteScalarAsync())!;
		return (int)result == 1;
	}

	public async Task<bool> UpdateStatusAsync(EventSchedule eventSchedule)
	{
		const string sql = @"
			UPDATE EventSchedules SET Status = @Status
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", eventSchedule.Id);
		command.Parameters.AddWithValue("@CompanyId", eventSchedule.CompanyId);
		command.Parameters.AddWithValue("@Status", eventSchedule.Status.ToString());

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<(int MaxParticipants, int CurrentParticipants)> GetMaxParticipantsAndCurrentParticipantsAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			SELECT 
				et.MaxParticipants,
				COUNT(rp.Id) AS CurrentParticipants
			FROM EventSchedules es
			INNER JOIN EventTypes et ON es.EventTypeId = et.Id
			LEFT JOIN Reservations r 
				ON es.Id = r.EventScheduleId 
				AND r.CompanyId = es.CompanyId
				AND r.Status <> @CancelledStatus
			LEFT JOIN ReservationParticipants rp 
				ON r.Id = rp.ReservationId AND rp.CompanyId = es.CompanyId
			WHERE es.Id = @EventScheduleId AND es.CompanyId = @CompanyId
			GROUP BY et.MaxParticipants";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@EventScheduleId", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@CancelledStatus", nameof(ReservationStatus.Cancelled));

		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		if (await reader.ReadAsync())
		{
			int maxParticipants = (int)reader["MaxParticipants"];
			int currentParticipants = (int)reader["CurrentParticipants"];

			return (maxParticipants, currentParticipants);
		}

		return (0, 0);
	}

	public async Task<bool> IsParticipantAssignedAsync(
		Guid participantId,
		Guid eventScheduleId)
	{
		const string sql = @"
			SELECT CASE WHEN EXISTS (
				SELECT 1 FROM ReservationParticipants rp
				INNER JOIN Reservations r ON rp.ReservationId = r.Id
				WHERE rp.ParticipantId = @ParticipantId
				AND r.EventScheduleId = @EventScheduleId
				AND r.Status <> @CancelledStatus
			) THEN 1 ELSE 0 END";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@ParticipantId", participantId);
		command.Parameters.AddWithValue("@EventScheduleId", eventScheduleId);
		command.Parameters.AddWithValue("@CancelledStatus", nameof(ReservationStatus.Cancelled));

		object result = (await command.ExecuteScalarAsync())!;
		return (int)result == 1;
	}
}