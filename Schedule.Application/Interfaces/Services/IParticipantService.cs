using Schedule.Contracts.Dtos.Responses;
using Schedule.Domain.Models;

namespace Schedule.Application.Interfaces.Services;

public interface IParticipantService
{
	Task<Guid> CreateAsync(Participant participant);

	Task PutAsync(Participant participant);

	Task DeleteByIdAsync(
		Guid participantId,
		Guid companyId);

	Task<Participant?> GetByIdAsync(
		Guid participantId,
		Guid companyId);

	Task<Participant?> GetByEmailAsync(
		string email,
		Guid companyId);

	Task<(List<Participant> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize,
		string? search);
}