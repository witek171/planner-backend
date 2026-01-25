using ReservationSystem.Domain.Models;

namespace ReservationSystem.Application.Interfaces.Repositories;

public interface IEventScheduleRepository
{
	Task<List<EventSchedule>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId);

	Task<(List<EventSchedule> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize,
		Guid? eventTypeId);

	Task<EventSchedule?> GetByIdAsync(
		Guid id,
		Guid companyId);

	Task<Guid> CreateAsync(EventSchedule eventSchedule);
	Task<bool> UpdateAsync(EventSchedule eventSchedule);

	Task<bool> DeleteAsync(
		Guid id,
		Guid companyId);

	Task<bool> HasRelatedRecordsAsync(
		Guid id,
		Guid companyId);

	Task<bool> UpdateStatusAsync(EventSchedule eventSchedule);

	Task<(int MaxParticipants, int CurrentParticipants)> GetMaxParticipantsAndCurrentParticipantsAsync(
		Guid id,
		Guid companyId);

	Task<bool> IsParticipantAssignedAsync(
		Guid participantId,
		Guid eventScheduleId);
}