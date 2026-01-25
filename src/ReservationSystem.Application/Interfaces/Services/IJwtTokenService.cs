using ReservationSystem.Domain.Models.Enums;

namespace ReservationSystem.Application.Interfaces.Services;

public interface IJwtTokenService
{
	string GenerateToken(Guid userId, StaffRole role);
}
