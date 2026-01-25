using Microsoft.Data.SqlClient;
using ReservationSystem.Application.Interfaces.Repositories;
using ReservationSystem.Domain.Models;

namespace ReservationSystem.Infrastructure.Repositories;

public class ReservationParticipantRepository : IReservationParticipantRepository
{
	private readonly string _connectionString;

	public ReservationParticipantRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<List<Guid>> GetIdsByReservationIdAsync(
		Guid reservationId,
		Guid companyId)
	{
		const string sql = @"
			SELECT Id
			FROM ReservationParticipants 
			WHERE ReservationId = @ReservationId AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@ReservationId", reservationId);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		SqlDataReader reader = await command.ExecuteReaderAsync();

		List<Guid> ids = new();
		while (await reader.ReadAsync())
			ids.Add(reader.GetGuid(reader.GetOrdinal("Id")));

		return ids;
	}

	public async Task<Guid> CreateAsync(ReservationParticipant reservationParticipant)
	{
		const string sql = @"
			INSERT INTO ReservationParticipants 
			(CompanyId, ReservationId, ParticipantId)
			OUTPUT INSERTED.Id
			VALUES 
			(@CompanyId, @ReservationId, @ParticipantId)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", reservationParticipant.CompanyId);
		command.Parameters.AddWithValue("@ReservationId", reservationParticipant.ReservationId);
		command.Parameters.AddWithValue("@ParticipantId", reservationParticipant.ParticipantId);

		object result = (await command.ExecuteScalarAsync())!;
		return (Guid)result;
	}

	public async Task<bool> DeleteAsync(
		Guid id,
		Guid companyId)
	{
		const string sql = @"
			DELETE FROM ReservationParticipants WHERE Id = @Id AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		Int32 affected = await command.ExecuteNonQueryAsync();
		return affected > 0;
	}
}