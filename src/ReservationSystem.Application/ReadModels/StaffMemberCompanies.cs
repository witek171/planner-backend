using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.ReadModels;

public record StaffMemberCompanies(
	StaffMember StaffMember,
	IReadOnlyList<Company> Companies
);