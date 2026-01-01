using Schedule.Domain.Models;

namespace Schedule.Application.Interfaces.Services;

public interface IReservationService
{
	Task<(List<Reservation> Items, int TotalCount)> GetAllAsync(
		Guid companyId,
		int page,
		int pageSize);

	Task<Reservation?> GetByIdAsync(
		Guid id,
		Guid companyId);

	Task<Guid> CreateAsync(Reservation reservation);
	Task UpdateAsync(Reservation reservation);

	Task SoftDeleteAsync(
		Guid id,
		Guid companyId);

	Task MarkAsPaidAsync(Reservation reservation);
	Task UnmarkAsPaidAsync(Reservation reservation);
}