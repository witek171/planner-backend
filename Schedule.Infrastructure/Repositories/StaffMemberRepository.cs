using Microsoft.Data.SqlClient;
using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.ReadModels;
using Schedule.Domain.Models;
using Schedule.Infrastructure.Utils;

namespace Schedule.Infrastructure.Repositories;

public class StaffMemberRepository : IStaffMemberRepository
{
	private readonly string _connectionString;

	public StaffMemberRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<Guid> CreateAsync(StaffMember staffMember)
	{
		const string sql = @"
			INSERT INTO Staff (Role, Email, Password, FirstName, LastName, Phone)
			OUTPUT INSERTED.Id
			VALUES (@Role, @Email, @Password, @FirstName, @LastName, @Phone)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Role", staffMember.Role.ToString());
		command.Parameters.AddWithValue("@Email", staffMember.Email);
		command.Parameters.AddWithValue("@Password", staffMember.Password);
		command.Parameters.AddWithValue("@FirstName", staffMember.FirstName);
		command.Parameters.AddWithValue("@LastName", staffMember.LastName);
		command.Parameters.AddWithValue("@Phone", staffMember.Phone);

		object result = (await command.ExecuteScalarAsync())!;
		Guid staffMemberId = (Guid)result;

		return staffMemberId;
	}

	public async Task<bool> PutAsync(StaffMember staffMember)
	{
		const string sql = @"
			UPDATE Staff SET
			Role = @Role,
			Email = @Email,
			Password = @Password,
			FirstName = @FirstName,
			LastName = @LastName,
			Phone = @Phone
			WHERE Id = @Id";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", staffMember.Id);
		command.Parameters.AddWithValue("@Role", staffMember.Role.ToString());
		command.Parameters.AddWithValue("@Email", staffMember.Email);
		command.Parameters.AddWithValue("@Password", staffMember.Password);
		command.Parameters.AddWithValue("@FirstName", staffMember.FirstName);
		command.Parameters.AddWithValue("@LastName", staffMember.LastName);
		command.Parameters.AddWithValue("@Phone", staffMember.Phone);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<bool> DeleteByIdAsync(Guid staffMemberId, Guid companyId)
	{
		const string staffCompanySql = @"
			DELETE FROM StaffMemberCompanies 
			WHERE StaffMemberId = @StaffMemberId AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand staffCompanyCommand = new(staffCompanySql, connection);
		staffCompanyCommand.Parameters.AddWithValue("@StaffMemberId", staffMemberId);
		staffCompanyCommand.Parameters.AddWithValue("@CompanyId", companyId);
		await staffCompanyCommand.ExecuteNonQueryAsync();

		return true;
	}

	public async Task<(List<StaffMember> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize)
	{
		const string staffSql = @"
			SELECT s.Id as StaffMemberId,
				s.Role,
				s.Email,
				s.Password,
				s.FirstName,
				s.LastName,
				s.Phone,
				s.CreatedAt,
				s.IsDeleted,
				COUNT(*) OVER() AS TotalCount
			FROM Staff s
			INNER JOIN StaffMemberCompanies sc ON s.Id = sc.StaffMemberId
			WHERE sc.CompanyId = @CompanyId AND s.IsDeleted = 0
			ORDER BY s.CreatedAt DESC
			OFFSET @Offset ROWS
			FETCH NEXT @PageSize ROWS ONLY";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		List<StaffMember> staffMembers = new();
		List<Guid> staffIds = new();
		int totalCount = 0;
		await using (SqlCommand command = new(staffSql, connection))
		{
			command.Parameters.AddWithValue("@CompanyId", companyId);
			command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
			command.Parameters.AddWithValue("@PageSize", pageSize);

			await using SqlDataReader reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				if (totalCount == 0)
					totalCount = Convert.ToInt32(reader["TotalCount"]);

				StaffMember staffMember = DbMapper.MapStaffMember(reader);
				staffMembers.Add(staffMember);
				staffIds.Add(staffMember.Id);
			}
		}

		if (staffIds.Count == 0)
			return (staffMembers, totalCount);

		string staffIdsParam = string.Join(",", staffIds);
		const string companiesSql = @"
			SELECT Id, StaffMemberId, CompanyId, CreatedAt 
			FROM StaffMemberCompanies 
			WHERE StaffMemberId IN 
				(SELECT CAST(value AS UNIQUEIDENTIFIER) FROM STRING_SPLIT(@StaffIds, ','))";

		await using (SqlCommand companiesCommand = new(companiesSql, connection))
		{
			companiesCommand.Parameters.AddWithValue("@StaffIds", staffIdsParam);
			await using SqlDataReader companiesReader = await companiesCommand.ExecuteReaderAsync();
			Dictionary<Guid, List<StaffMemberCompany>> companiesDict =
				staffMembers.ToDictionary(sm => sm.Id, _ => new List<StaffMemberCompany>());

			while (await companiesReader.ReadAsync())
			{
				Guid staffId = companiesReader.GetGuid(companiesReader.GetOrdinal("StaffMemberId"));
				StaffMemberCompany staffCompany = new(
					companiesReader.GetGuid(companiesReader.GetOrdinal("Id")),
					staffId,
					companiesReader.GetGuid(companiesReader.GetOrdinal("CompanyId")),
					companiesReader.GetDateTime(companiesReader.GetOrdinal("CreatedAt")));

				if (companiesDict.TryGetValue(staffId, out List<StaffMemberCompany>? companies))
					companies.Add(staffCompany);
			}

			foreach (StaffMember staffMember in staffMembers)
				staffMember.SetStaffMemberCompanies(companiesDict[staffMember.Id]);
		}

		const string specializationsSql = @"
			SELECT 
				sms.StaffMemberId,
				sp.Id,
				sp.CompanyId,
				sp.Name,
				sp.Description
			FROM StaffMemberSpecializations sms
			INNER JOIN Specializations sp ON sms.SpecializationId = sp.Id
			WHERE sms.StaffMemberId IN 
			    (SELECT CAST(value AS UNIQUEIDENTIFIER) FROM STRING_SPLIT(@StaffIds, ','))
			AND sms.CompanyId = @CompanyId";

		await using (SqlCommand specCommand = new(specializationsSql, connection))
		{
			specCommand.Parameters.AddWithValue("@StaffIds", staffIdsParam);
			specCommand.Parameters.AddWithValue("@CompanyId", companyId);

			await using SqlDataReader specReader = await specCommand.ExecuteReaderAsync();
			Dictionary<Guid, List<Specialization>> specDict =
				staffMembers.ToDictionary(sm => sm.Id, _ => new List<Specialization>());

			while (await specReader.ReadAsync())
			{
				Guid staffId = specReader.GetGuid(specReader.GetOrdinal("StaffMemberId"));
				Specialization specialization = new(
					specReader.GetGuid(specReader.GetOrdinal("Id")),
					specReader.GetGuid(specReader.GetOrdinal("CompanyId")),
					specReader.GetString(specReader.GetOrdinal("Name")),
					specReader.GetString(specReader.GetOrdinal("Description")));

				if (specDict.TryGetValue(staffId, out List<Specialization>? specs))
					specs.Add(specialization);
			}

			foreach (StaffMember staffMember in staffMembers)
			{
				List<Specialization> specs = specDict[staffMember.Id];
				if (specs.Any())
					staffMember.SetSpecializations(specs);
			}
		}

		return (staffMembers, totalCount);
	}

	public async Task<StaffMember?> GetByIdAsync(Guid staffMemberId, Guid companyId)
	{
		const string sql = @"
			SELECT s.Id as StaffMemberId, s.Role, s.Email, s.Password, s.FirstName, s.LastName, s.Phone, s.CreatedAt, s.IsDeleted
			FROM Staff s
			INNER JOIN StaffMemberCompanies sc ON s.Id = sc.StaffMemberId
			WHERE s.Id = @Id AND sc.CompanyId = @CompanyId AND s.IsDeleted = 0";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();
		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", staffMemberId);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		StaffMember? staffMember = null;
		if (await reader.ReadAsync())
			staffMember = DbMapper.MapStaffMember(reader);

		reader.Close();

		if (staffMember != null)
			staffMember = await AttachStaffMemberCompaniesAsync(staffMember, connection);

		return staffMember;
	}

	public async Task<StaffMember?> GetByEmailAsync(string email)
	{
		const string sql = @"
			SELECT s.Id as StaffMemberId, s.Role, s.Email, s.Password, s.FirstName, s.LastName, s.Phone, s.CreatedAt, s.IsDeleted
			FROM Staff s
			WHERE s.Email = @Email AND s.IsDeleted = 0";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();
		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Email", email);
		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		StaffMember? staffMember = null;
		if (await reader.ReadAsync())
			staffMember = DbMapper.MapStaffMember(reader);

		reader.Close();

		if (staffMember != null)
			staffMember = await AttachStaffMemberCompaniesAsync(staffMember, connection);

		return staffMember;
	}

	private async Task<StaffMember> AttachStaffMemberCompaniesAsync(StaffMember staffMember, SqlConnection connection)
	{
		const string sql = @"
			SELECT Id, StaffMemberId, CompanyId, CreatedAt 
			FROM StaffMemberCompanies WHERE StaffMemberId = @StaffMemberId";

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@StaffMemberId", staffMember.Id);
		await using SqlDataReader reader = await command.ExecuteReaderAsync();

		List<StaffMemberCompany> companies = new();
		while (await reader.ReadAsync())
		{
			companies.Add(new StaffMemberCompany(
				reader.GetGuid(reader.GetOrdinal("Id")),
				reader.GetGuid(reader.GetOrdinal("StaffMemberId")),
				reader.GetGuid(reader.GetOrdinal("CompanyId")),
				reader.GetDateTime(reader.GetOrdinal("CreatedAt"))));
		}

		staffMember = new StaffMember(
			staffMember.Id,
			staffMember.Role,
			staffMember.Email,
			staffMember.Password,
			staffMember.FirstName,
			staffMember.LastName,
			staffMember.Phone,
			staffMember.CreatedAt,
			staffMember.IsDeleted,
			staffMember.Specializations.ToList(),
			companies);

		return staffMember;
	}

	public async Task<bool> HasRelatedRecordsAsync(Guid staffMemberId, Guid companyId)
	{
		const string sql = @"
			SELECT CASE 
			WHEN EXISTS (
				SELECT 1 FROM EventScheduleStaff WHERE StaffMemberId = @StaffMemberId AND CompanyId = @CompanyId
			) 
			OR EXISTS (
				SELECT 1 FROM StaffAvailability WHERE StaffMemberId = @StaffMemberId AND CompanyId = @CompanyId  
			)
			OR EXISTS (
				SELECT 1 FROM StaffSpecializations WHERE StaffMemberId = @StaffMemberId AND CompanyId = @CompanyId
			)
			OR EXISTS (
				SELECT 1 FROM Messages WHERE SenderId = @StaffMemberId AND CompanyId = @CompanyId
			)
			THEN 1 ELSE 0 END";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		int result = (int)(await command.ExecuteScalarAsync())!;
		return result == 1;
	}

	public async Task<bool> EmailExistsForOtherAsync(Guid companyId, Guid staffMemberId, string email)
	{
		const string sql = @"
			SELECT 1 
			FROM Staff s
			INNER JOIN StaffMemberCompanies sc ON s.Id = sc.StaffMemberId
			WHERE sc.CompanyId = @CompanyId 
			AND s.Email = @Email 
			AND s.Id <> @StaffMemberId
			AND s.isDeleted = 0";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@Email", email);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);

		object? result = await command.ExecuteScalarAsync();
		return result != null;
	}

	public async Task<bool> PhoneExistsForOtherAsync(Guid companyId, Guid staffMemberId, string phone)
	{
		const string sql = @"
			SELECT 1 
			FROM Staff s
			INNER JOIN StaffMemberCompanies sc ON s.Id = sc.StaffMemberId
			WHERE sc.CompanyId = @CompanyId 
			AND s.Phone = @Phone 
			AND s.Id <> @StaffMemberId
			AND s.isDeleted = 0";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		command.Parameters.AddWithValue("@Phone", phone);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);

		object? result = await command.ExecuteScalarAsync();
		return result != null;
	}

	public async Task<bool> EmailExistsForOtherWithoutCompanyIdAsync(Guid staffMemberId, string email)
	{
		const string sql = @"
			SELECT 1 
			FROM Staff s
			WHERE s.Email = @Email 
			AND s.Id <> @StaffMemberId
			AND s.isDeleted = 0";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Email", email);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);

		object? result = await command.ExecuteScalarAsync();
		return result != null;
	}

	public async Task<bool> PhoneExistsForOtherWithoutCompanyIdAsync(Guid staffMemberId, string phone)
	{
		const string sql = @"
			SELECT 1 
			FROM Staff s
			WHERE s.Phone = @Phone 
			AND s.Id <> @StaffMemberId
			AND s.isDeleted = 0";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Phone", phone);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);

		object? result = await command.ExecuteScalarAsync();
		return result != null;
	}

	public async Task<bool> UpdateSoftDeleteAsync(StaffMember staffMember)
	{
		const string sql = @"
			UPDATE Staff SET
			IsDeleted = @IsDeleted
			WHERE Id = @Id";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@Id", staffMember.Id);
		command.Parameters.AddWithValue("@IsDeleted", staffMember.IsDeleted);

		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<Guid> AssignToCompanyAsync(Guid staffMemberId, Guid companyId)
	{
		const string sql = @"
			INSERT INTO StaffMemberCompanies (StaffMemberId, CompanyId) 
			OUTPUT INSERTED.Id
			VALUES (@StaffMemberId, @CompanyId)";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);
		command.Parameters.AddWithValue("@CompanyId", companyId);

		object result = (await command.ExecuteScalarAsync())!;
		Guid staffMemberCompaniesId = (Guid)result;

		return staffMemberCompaniesId;
	}

	public async Task<bool> UnassignFromCompanyAsync(Guid staffMemberId, Guid companyId)
	{
		const string sql = @"
			DELETE FROM StaffMemberCompanies 
			WHERE StaffMemberId = @StaffMemberId AND CompanyId = @CompanyId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();
		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);
		command.Parameters.AddWithValue("@CompanyId", companyId);
		int rowsAffected = await command.ExecuteNonQueryAsync();
		return rowsAffected > 0;
	}

	public async Task<StaffMemberCompanies> GetAssignedCompanyAsync(Guid staffMemberId)
	{
		const string sql = @"
			SELECT 
				s.Id as StaffMemberId, s.Role, s.Email, s.Password, s.FirstName, s.LastName, s.Phone, s.CreatedAt, s.IsDeleted
			FROM Staff s
			WHERE s.Id = @StaffMemberId AND s.IsDeleted = 0;

			SELECT DISTINCT
				c.Id, c.Name, c.TaxCode, c.Street, c.City, c.PostalCode,
				c.Phone, c.Email, c.IsParentNode, c.IsReception, c.CreatedAt
			FROM Companies c
			INNER JOIN StaffMemberCompanies smc ON c.Id = smc.CompanyId
			WHERE smc.StaffMemberId = @StaffMemberId";

		await using SqlConnection connection = new(_connectionString);
		await connection.OpenAsync();

		await using SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@StaffMemberId", staffMemberId);

		await using SqlDataReader reader = await command.ExecuteReaderAsync();
		await reader.ReadAsync();
		StaffMember staffMember = DbMapper.MapStaffMember(reader);

		List<Company> companies = [];
		await reader.NextResultAsync();
		while (await reader.ReadAsync())
			companies.Add(DbMapper.MapCompany(reader));

		return new StaffMemberCompanies(staffMember, companies);
	}
}