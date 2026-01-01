using Schedule.Domain.Models;

namespace Schedule.Application.Interfaces.Repositories;

public interface IReservationRepository
{
	Task<(List<Reservation> Items, int TotalCount)> GetPagedWithCountAsync(
		Guid companyId,
		int page,
		int pageSize);

	Task<Reservation?> GetByIdAsync(
		Guid id,
		Guid companyId);

	Task<Guid> CreateAsync(Reservation reservation);
	Task<bool> UpdateAsync(Reservation reservation);

	Task<bool> UpdateSoftDeleteAsync(Reservation reservation);
	Task<bool> UpdatePaymentDetailsAsync(Reservation reservation);

	Task<List<Guid>> GetIdsByEventScheduleIdAsync(
		Guid eventScheduleId,
		Guid companyId);

	Task<List<Reservation>> GetByParticipantIdAsync(
		Guid participantId,
		Guid companyId);
}