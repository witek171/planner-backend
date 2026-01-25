using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Repositories;

public interface IParticipantRepository
{
	Task<Guid> CreateAsync(Participant participant);
	Task<bool> PutAsync(Participant participant);

	Task<bool> DeleteByIdAsync(
		Guid participantId,
		Guid companyId);

	Task<Participant?> GetByIdAsync(
		Guid participantId,
		Guid companyId);

	Task<Participant?> GetByEmailAsync(
		string email,
		Guid companyId);

	Task<(List<Participant> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize,
		string? search);

	Task<bool> IsParticipantAssignedToReservationsAsync(
		Guid participantId,
		Guid companyId);
}