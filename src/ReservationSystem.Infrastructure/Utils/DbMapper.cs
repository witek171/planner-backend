using Microsoft.Data.SqlClient;
using ReservationSystem.Domain.Models;
using ReservationSystem.Domain.Models.Enums;

namespace ReservationSystem.Infrastructure.Utils;

public static class DbMapper
{
	public static Company MapCompany(SqlDataReader reader)
	{
		return new Company(
			reader.GetGuid(reader.GetOrdinal("Id")),
			reader.GetString(reader.GetOrdinal("Name")),
			reader.GetString(reader.GetOrdinal("TaxCode")),
			reader.GetString(reader.GetOrdinal("Street")),
			reader.GetString(reader.GetOrdinal("City")),
			reader.GetString(reader.GetOrdinal("PostalCode")),
			reader.GetString(reader.GetOrdinal("Phone")),
			reader.GetString(reader.GetOrdinal("Email")),
			reader.GetBoolean(reader.GetOrdinal("IsParentNode")),
			reader.GetBoolean(reader.GetOrdinal("IsReception")),
			reader.GetDateTime(reader.GetOrdinal("CreatedAt")));
	}

	public static EventSchedule MapEventSchedule(SqlDataReader reader)
	{
		EventType eventType = new(
			reader.GetGuid(reader.GetOrdinal("EventTypeId")),
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetString(reader.GetOrdinal("EventTypeName")),
			reader.GetString(reader.GetOrdinal("EventTypeDescription")),
			reader.GetInt32(reader.GetOrdinal("Duration")),
			reader.GetDecimal(reader.GetOrdinal("Price")),
			reader.GetInt32(reader.GetOrdinal("MaxParticipants")),
			reader.GetInt32(reader.GetOrdinal("MinStaff")),
			reader.GetBoolean(reader.GetOrdinal("EventTypeIsDeleted")));

		return new EventSchedule(
			reader.GetGuid(reader.GetOrdinal("Id")),
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetGuid(reader.GetOrdinal("EventTypeId")),
			eventType,
			reader.GetString(reader.GetOrdinal("PlaceName")),
			reader.GetDateTime(reader.GetOrdinal("StartTime")),
			reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
			Enum.Parse<EventScheduleStatus>(reader.GetString(reader.GetOrdinal("Status"))));
	}

	public static EventScheduleStaffMember MapEventScheduleStaffMember(SqlDataReader reader)
	{
		return new EventScheduleStaffMember(
			reader.GetGuid(reader.GetOrdinal("Id")),
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetGuid(reader.GetOrdinal("EventScheduleId")),
			reader.GetGuid(reader.GetOrdinal("StaffMemberId")));
	}

	public static Participant MapParticipant(SqlDataReader reader)
	{
		return new Participant(
			reader.GetGuid(reader.GetOrdinal("Id")),
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetString(reader.GetOrdinal("Email")),
			reader.GetString(reader.GetOrdinal("FirstName")),
			reader.GetString(reader.GetOrdinal("LastName")),
			reader.GetString(reader.GetOrdinal("Phone")),
			reader.GetBoolean(reader.GetOrdinal("GdprConsent")),
			reader.GetDateTime(reader.GetOrdinal("CreatedAt")));
	}

	public static Specialization MapSpecialization(SqlDataReader reader)
	{
		return new Specialization(
			reader.GetGuid(reader.GetOrdinal("Id")),
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetString(reader.GetOrdinal("Name")),
			reader.GetString(reader.GetOrdinal("Description")));
	}

	public static StaffMemberAvailability MapStaffMemberAvailability(SqlDataReader reader)
	{
		return new StaffMemberAvailability(
			reader.GetGuid(reader.GetOrdinal("Id")),
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetGuid(reader.GetOrdinal("StaffMemberId")),
			DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Date"))),
			reader.GetDateTime(reader.GetOrdinal("StartTime")),
			reader.GetDateTime(reader.GetOrdinal("EndTime")),
			reader.GetBoolean(reader.GetOrdinal("IsAvailable")));
	}

	public static StaffMember MapStaffMember(SqlDataReader reader)
	{
		return new StaffMember(
			reader.GetGuid(reader.GetOrdinal("StaffMemberId")),
			Enum.Parse<StaffRole>(reader.GetString(reader.GetOrdinal("Role"))),
			reader.GetString(reader.GetOrdinal("Email")),
			reader.GetString(reader.GetOrdinal("Password")),
			reader.GetString(reader.GetOrdinal("FirstName")),
			reader.GetString(reader.GetOrdinal("LastName")),
			reader.GetString(reader.GetOrdinal("Phone")),
			reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
			reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
			new List<Specialization>(),
			new List<StaffMemberCompany>());
	}

	public static EventType MapEventType(SqlDataReader reader)
	{
		return new EventType(
			reader.GetGuid(reader.GetOrdinal("Id")),
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetString(reader.GetOrdinal("Name")),
			reader.GetString(reader.GetOrdinal("Description")),
			reader.GetInt32(reader.GetOrdinal("Duration")),
			reader.GetDecimal(reader.GetOrdinal("Price")),
			reader.GetInt32(reader.GetOrdinal("MaxParticipants")),
			reader.GetInt32(reader.GetOrdinal("MinStaff")),
			reader.GetBoolean(reader.GetOrdinal("IsDeleted")));
	}

	public static Reservation MapReservation(SqlDataReader reader)
	{
		EventType eventType = new(
			reader.GetGuid(reader.GetOrdinal("EventTypeId")),
			reader.GetGuid(reader.GetOrdinal("EventTypeCompanyId")),
			reader.GetString(reader.GetOrdinal("EventTypeName")),
			reader.GetString(reader.GetOrdinal("EventTypeDescription")),
			reader.GetInt32(reader.GetOrdinal("Duration")),
			reader.GetDecimal(reader.GetOrdinal("Price")),
			reader.GetInt32(reader.GetOrdinal("MaxParticipants")),
			reader.GetInt32(reader.GetOrdinal("MinStaff")),
			reader.GetBoolean(reader.GetOrdinal("EventTypeIsDeleted")));

		EventSchedule eventSchedule = new(
			reader.GetGuid(reader.GetOrdinal("EventScheduleId")),
			reader.GetGuid(reader.GetOrdinal("EventScheduleCompanyId")),
			reader.GetGuid(reader.GetOrdinal("EventTypeId")),
			eventType,
			reader.GetString(reader.GetOrdinal("PlaceName")),
			reader.GetDateTime(reader.GetOrdinal("StartTime")),
			reader.GetDateTime(reader.GetOrdinal("EventScheduleCreatedAt")),
			Enum.Parse<EventScheduleStatus>(reader.GetString(reader.GetOrdinal("EventScheduleStatus"))));

		return new Reservation(
			reader.GetGuid(reader.GetOrdinal("ReservationId")),
			reader.GetGuid(reader.GetOrdinal("ReservationCompanyId")),
			reader.GetGuid(reader.GetOrdinal("EventScheduleId")),
			eventSchedule,
			new List<Guid>(),
			new List<Participant>(),
			Enum.Parse<ReservationStatus>(reader.GetString(reader.GetOrdinal("Status"))),
			reader.GetString(reader.GetOrdinal("Notes")),
			reader.GetDateTime(reader.GetOrdinal("ReservationCreatedAt")),
			reader.IsDBNull(reader.GetOrdinal("CancelledAt"))
				? null
				: reader.GetDateTime(reader.GetOrdinal("CancelledAt")),
			reader.GetBoolean(reader.GetOrdinal("IsPaid")),
			reader.IsDBNull(reader.GetOrdinal("PaidAt"))
				? null
				: reader.GetDateTime(reader.GetOrdinal("PaidAt")));
	}

	public static Participant MapParticipantFromReservation(SqlDataReader reader)
	{
		return new Participant(
			reader.GetGuid(reader.GetOrdinal("ParticipantId")),
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetString(reader.GetOrdinal("Email")),
			reader.GetString(reader.GetOrdinal("FirstName")),
			reader.GetString(reader.GetOrdinal("LastName")),
			reader.GetString(reader.GetOrdinal("Phone")),
			reader.GetBoolean(reader.GetOrdinal("GdprConsent")),
			reader.GetDateTime(reader.GetOrdinal("CreatedAt")));
	}

	public static CompanyConfig MapCompanyConfig(SqlDataReader reader)
	{
		return new CompanyConfig(
			reader.GetGuid(reader.GetOrdinal("CompanyId")),
			reader.GetInt32(reader.GetOrdinal("BreakTimeStaff")),
			reader.GetInt32(reader.GetOrdinal("BreakTimeParticipants")));
	}
}