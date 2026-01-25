namespace ReservationSystem.Application.Interfaces.Validators;

public interface IScheduleConflictValidator
{
	Task<bool> CanAssignStaffMemberAsync(
		Guid companyId,
		Guid staffMemberId,
		DateTime start,
		DateTime end);

	Task<bool> CanAssignParticipantAsync(
		Guid companyId,
		Guid participantId,
		DateTime start,
		DateTime end);
}