using Schedule.Domain.Models;

namespace Schedule.Application.ReadModels;

public record StaffMemberCompanies(
	StaffMember StaffMember,
	IReadOnlyList<Company> Companies
);