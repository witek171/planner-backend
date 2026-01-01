using Microsoft.Data.SqlClient;
using Schedule.Application.Interfaces.Repositories;
using Schedule.Domain.Models;
using Schedule.Domain.Models.Enums;
using Schedule.Infrastructure.Utils;

namespace Schedule.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
	private readonly string _connectionString;

	public ReservationRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<(List<Reservation> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize)
	{
		const string reservationsSql = @"
			SELECT 
				r.Id as ReservationId, 
				r.CompanyId as ReservationCompanyId, 
				r.EventScheduleId, 
				r.Status,
				r.Notes, 
				r.CreatedAt as ReservationCreatedAt, 
				r.CancelledAt, 
				r.IsPaid,
				r.PaidAt,
				es.Id as EventScheduleId,
				es.CompanyId as EventScheduleCompanyId,
				es.EventTypeId,
				es.PlaceName,
				es.StartTime,
				es.CreatedAt as EventScheduleCreatedAt,
				es.Status as EventScheduleStatus,
				et.Id as EventTypeId,
				et.CompanyId as EventTypeCompanyId,
				et.Name as EventTypeName,
				et.Description as EventTypeDescription,
				et.Duration,
				et.Price,
				et.MaxParticipants,
				et.MinStaff,
				et.IsDeleted as EventTypeIsDeleted,
				COUNT(*) OVER() AS TotalCount
			FROM Reservations r
			INNER JOIN EventSchedules es ON r.EventScheduleId = es.Id
			INNER JOIN EventTypes et ON es.EventTypeId = et.Id
			WHERE r.CompanyId = @CompanyId AND r.Status <> @CancelledStatus
			ORDER BY r.CreatedAt DESC
			OFFSET @Offset ROWS
			FETCH NEXT @PageSize ROWS ONLY";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		Dictionary<Guid, Reservation> reservationsDict = new();
		int totalCount = 0;
		await using (SqlCommand command = new(reservationsSql, connection))
		{
			command.Parameters.AddWithValue("@CompanyId", companyId);
			command.Parameters.AddWithValue("@CancelledStatus", nameof(ReservationStatus.Cancelled));
			command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
			command.Parameters.AddWithValue("@PageSize", pageSize);

			await using SqlDataReader reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				if (totalCount == 0)
					totalCount = Convert.ToInt32(reader["TotalCount"]);

				Reservation reservation = DbMapper.MapReservation(reader);
				reservationsDict[reservation.Id] = reservation;
			}
		}

		if (reservationsDict.Count > 0)
		{
			const string participantsSql = @"
				SELECT 
					rp.ReservationId,
					p.Id as ParticipantId,
					p.CompanyId,
					p.Email,
					p.FirstName,
					p.LastName,
					p.Phone,
					p.GdprConsent,
					p.CreatedAt
				FROM ReservationParticipants rp
				INNER JOIN Participants p ON rp.ParticipantId = p.Id
				WHERE rp.ReservationId IN (SELECT value FROM STRING_SPLIT(@ReservationIds, ','))";

			await using SqlCommand command = new(participantsSql, connection);
			string reservationIds = string.Join(",", reservationsDict.Keys);
			command.Parameters.AddWithValue("@ReservationIds", reservationIds);

			await using SqlDataReader reader = await command.ExecuteReaderAsync();
			Dictionary<Guid, List<Participant>> participantsDict = new();
			while (await reader.ReadAsync())
			{
				Guid reservationId = reader.GetGuid(reader.GetOrdinal("ReservationId"));
				Participant participant = DbMapper.MapParticipantFromReservation(reader);
				if (!participantsDict.ContainsKey(reservationId))
					participantsDict[reservationId] = new List<Participant>();

				participantsDict[reservationId].Add(participant);
			}

			foreach (KeyValuePair<Guid, List<Participant>> kvp in participantsDict)
			{
				if (reservationsDict.TryGetValue(kvp.Key, out Reservation? reservation))
					reservation.SetParticipants(kvp.Value);
			}
		}

		return (reservationsDict.Values.ToList(), totalCount);
	}

	public async Task<Reservation?> GetByIdAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			SELECT 
			r.Id as ReservationId, 
			r.CompanyId as ReservationCompanyId, 
			r.EventScheduleId, 
			r.Status,
			r.Notes, 
			r.CreatedAt as ReservationCreatedAt, 
			r.CancelledAt, 
			r.IsPaid,
			r.PaidAt,
			p.Id as ParticipantId,
			p.CompanyId as CompanyId,
			p.Email,
			p.FirstName,
			p.LastName,
			p.Phone,
			p.GdprConsent,
			p.CreatedAt as CreatedAt,
			es.Id as EventScheduleId,
			es.CompanyId as EventScheduleCompanyId,
			es.EventTypeId,
			es.PlaceName,
			es.StartTime,
			es.CreatedAt as EventScheduleCreatedAt,
			es.Status as EventScheduleStatus,
			et.Id as EventTypeId,
			et.CompanyId as EventTypeCompanyId,
			et.Name as EventTypeName,
			et.Description as EventTypeDescription,
			et.Duration,
			et.Price,
			et.MaxParticipants,
			et.MinStaff,
			et.IsDeleted as EventTypeIsDeleted

		FROM Reservations r
		LEFT JOIN ReservationParticipants rp ON r.Id = rp.ReservationId
		LEFT JOIN Participants p ON rp.ParticipantId = p.Id
		INNER JOIN EventSchedules es ON r.EventScheduleId = es.Id
		INNER JOIN EventTypes et ON es.EventTypeId = et.Id
		WHERE r.Id = @Id AND r.CompanyId = @CompanyId AND r.Status <> @CancelledStatus";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@CancelledStatus", nameof(ReservationStatus.Cancelled));

		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		Reservation? reservation = null;
		List<Participant> participants = new();

		while (await reader.ReadAsync())
		{
			reservation ??= DbMapper.MapReservation(reader);

			if (!reader.IsDBNull(reader.GetOrdinal("ParticipantId")))
				participants.Add(DbMapper.MapParticipantFromReservation(reader));
		}

		reservation?.SetParticipants(participants);

		return reservation;
	}

	public async Task<Guid> CreateAsync(Reservation reservation)
	{
		const string sql = @"
			INSERT INTO Reservations (CompanyId, EventScheduleId, Notes, IsPaid, PaidAt)
			OUTPUT INSERTED.Id
			VALUES (@CompanyId, @EventScheduleId, @Notes, @IsPaid, @PaidAt)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", reservation.CompanyId);
		command.Parameters.AddWithValue("@EventScheduleId", reservation.EventScheduleId);
		command.Parameters.AddWithValue("@Notes", reservation.Notes);
		command.Parameters.AddWithValue("@IsPaid", reservation.IsPaid);
		command.Parameters.AddWithValue("@PaidAt", reservation.PaidAt ?? (object)DBNull.Value);

		object result = (await command.ExecuteScalarAsync())!;
		return (Guid)result;
	}

	public async Task<bool> UpdateAsync(Reservation reservation)
	{
		const string sql = @"
			UPDATE Reservations 
			SET Notes = @Notes
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", reservation.Id);
		command.Parameters.AddWithValue("@CompanyId", reservation.CompanyId);
		command.Parameters.AddWithValue("@Notes", reservation.Notes);

		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}

	public async Task<bool> UpdateSoftDeleteAsync(Reservation reservation)
	{
		const string sql = @"
			UPDATE Reservations SET
			Status = @Status, CancelledAt = @CancelledAt
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", reservation.Id);
		command.Parameters.AddWithValue("@CompanyId", reservation.CompanyId);
		command.Parameters.AddWithValue("@Status", reservation.Status.ToString());
		command.Parameters.AddWithValue("@CancelledAt", reservation.CancelledAt);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<bool> UpdatePaymentDetailsAsync(Reservation reservation)
	{
		const string sql = @"
			UPDATE Reservations SET
			IsPaid = @IsPaid, PaidAt = @PaidAt
			WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", reservation.Id);
		command.Parameters.AddWithValue("@CompanyId", reservation.CompanyId);
		command.Parameters.AddWithValue("@IsPaid", reservation.IsPaid);
		command.Parameters.AddWithValue("@PaidAt", reservation.PaidAt ?? (object)DBNull.Value);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<List<Guid>> GetIdsByEventScheduleIdAsync(
		Guid eventScheduleId,
		Guid companyId)
	{
		const string sql = @"
			SELECT id
			FROM Reservations
			WHERE CompanyId = @CompanyId AND EventScheduleId = @EventScheduleId
			AND Status <> @CancelledStatus";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@EventScheduleId", eventScheduleId);
		command.Parameters.AddWithValue("@CancelledStatus", nameof(ReservationStatus.Cancelled));

		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		List<Guid> ids = new();
		while (await reader.ReadAsync())
			ids.Add(reader.GetGuid(reader.GetOrdinal("Id")));

		return ids;
	}

	public async Task<List<Reservation>> GetByParticipantIdAsync(
		Guid participantId,
		Guid companyId)
	{
		const string sql = @"
			SELECT 
			r.Id as ReservationId, 
			r.CompanyId as ReservationCompanyId, 
			r.EventScheduleId, 
			r.Status,
			r.Notes, 
			r.CreatedAt as ReservationCreatedAt, 
			r.CancelledAt, 
			r.IsPaid,
			r.PaidAt,
			p.Id as ParticipantId,
			p.CompanyId as CompanyId,
			p.Email,
			p.FirstName,
			p.LastName,
			p.Phone,
			p.GdprConsent,
			p.CreatedAt as CreatedAt,
			es.Id as EventScheduleId,
			es.CompanyId as EventScheduleCompanyId,
			es.EventTypeId,
			es.PlaceName,
			es.StartTime,
			es.CreatedAt as EventScheduleCreatedAt,
			es.Status as EventScheduleStatus,
			et.Id as EventTypeId,
			et.CompanyId as EventTypeCompanyId,
			et.Name as EventTypeName,
			et.Description as EventTypeDescription,
			et.Duration,
			et.Price,
			et.MaxParticipants,
			et.MinStaff,
			et.IsDeleted as EventTypeIsDeleted

		FROM Reservations r
		LEFT JOIN ReservationParticipants rp ON r.Id = rp.ReservationId
		LEFT JOIN Participants p ON rp.ParticipantId = p.Id
		INNER JOIN EventSchedules es ON r.EventScheduleId = es.Id
		INNER JOIN EventTypes et ON es.EventTypeId = et.Id
		WHERE p.Id = @ParticipantId AND r.CompanyId = @CompanyId 
		AND r.Status <> @CancelledStatus
		ORDER BY r.CreatedAt";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@ParticipantId", participantId);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@CancelledStatus", nameof(ReservationStatus.Cancelled));

		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		Dictionary<Guid, (Reservation reservation, List<Participant> participants)> reservationData = new();

		while (await reader.ReadAsync())
		{
			Guid reservationId = reader.GetGuid(reader.GetOrdinal("ReservationId"));

			if (!reservationData.ContainsKey(reservationId))
			{
				Reservation reservation = DbMapper.MapReservation(reader);
				reservationData[reservationId] = (reservation, new List<Participant>());
			}

			if (!reader.IsDBNull(reader.GetOrdinal("ParticipantId")))
			{
				Participant participant = DbMapper.MapParticipantFromReservation(reader);
				reservationData[reservationId].participants.Add(participant);
			}
		}

		List<Reservation> reservations = new();
		foreach ((Reservation reservation, List<Participant> participants) in reservationData.Values)
		{
			reservation.SetParticipants(participants);
			reservations.Add(reservation);
		}

		return reservations;
	}
}