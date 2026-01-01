using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class ParticipantService : IParticipantService
{
	private readonly IParticipantRepository _participantRepository;

	public ParticipantService(IParticipantRepository participantRepository)
	{
		_participantRepository = participantRepository;
	}

	public async Task<Guid> CreateAsync(Participant participant)
	{
		if (!participant.GdprConsent)
			throw new InvalidOperationException("GDPR consent is required");

		participant.Normalize();
		return await _participantRepository.CreateAsync(participant);
	}

	public async Task PutAsync(Participant participant)
	{
		participant.Normalize();
		await _participantRepository.PutAsync(participant);
	}

	public async Task DeleteByIdAsync(
		Guid participantId,
		Guid companyId)
	{
		if (await _participantRepository
				.IsParticipantAssignedToReservationsAsync(participantId, companyId))
		{
			Participant participant = (await _participantRepository
				.GetByIdAsync(participantId, companyId))!;
			participant.Anonymize();
			await _participantRepository.PutAsync(participant);
		}
		else
			await _participantRepository.DeleteByIdAsync(participantId, companyId);
	}

	public async Task<Participant?> GetByIdAsync(
		Guid participantId,
		Guid companyId)
		=> await _participantRepository.GetByIdAsync(participantId, companyId);

	public async Task<Participant?> GetByEmailAsync(
		string email,
		Guid companyId)
	{
		email = email.Trim().ToLowerInvariant();
		return await _participantRepository.GetByEmailAsync(email, companyId);
	}

	public async Task<(List<Participant> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize)
		=> await _participantRepository.GetPagedWithCountAsync(companyId, page, pageSize);
}