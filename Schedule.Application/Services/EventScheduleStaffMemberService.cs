using Schedule.Application.Interfaces.Repositories;
using Schedule.Application.Interfaces.Services;
using Schedule.Application.Interfaces.Validators;
using Schedule.Domain.Exceptions;
using Schedule.Domain.Models;

namespace Schedule.Application.Services;

public class EventScheduleStaffMemberService : IEventScheduleStaffMemberService
{
	private readonly IEventScheduleStaffMemberRepository _eventScheduleStaffMemberRepository;
	private readonly IScheduleConflictValidator _scheduleConflictValidator;
	private readonly IEventScheduleRepository _eventScheduleRepository;
	private readonly IStaffMemberRepository _staffMemberRepository;

	public EventScheduleStaffMemberService(
		IEventScheduleStaffMemberRepository eventScheduleStaffMemberRepository,
		IScheduleConflictValidator scheduleConflictValidator,
		IEventScheduleRepository eventScheduleRepository,
		IStaffMemberRepository staffMemberRepository)
	{
		_eventScheduleStaffMemberRepository = eventScheduleStaffMemberRepository;
		_scheduleConflictValidator = scheduleConflictValidator;
		_eventScheduleRepository = eventScheduleRepository;
		_staffMemberRepository = staffMemberRepository;
	}

	public async Task<List<EventScheduleStaffMember>> GetByStaffMemberIdAsync(
		Guid companyId,
		Guid staffMemberId)
		=> await _eventScheduleStaffMemberRepository
			.GetByStaffMemberIdAsync(companyId, staffMemberId);

	public async Task<Guid> CreateAsync(EventScheduleStaffMember eventScheduleStaffMember)
	{
		await ValidateEventScheduleAsync(eventScheduleStaffMember);
		await ValidateStaffMemberAsync(eventScheduleStaffMember);
		Guid companyId = eventScheduleStaffMember.CompanyId;
		Guid staffMemberId = eventScheduleStaffMember.StaffMemberId;
		Guid eventScheduleId = eventScheduleStaffMember.EventScheduleId;
		EventSchedule eventSchedule = (await _eventScheduleRepository
			.GetByIdAsync(eventScheduleId, companyId))!;
		DateTime startTime = eventSchedule.StartTime;
		DateTime endTime = eventSchedule.EndTime;

		if (!await _scheduleConflictValidator
				.CanAssignStaffMemberAsync(companyId, staffMemberId, startTime, endTime))
			throw new StaffMemberTimeConflictException(staffMemberId);

		return await _eventScheduleStaffMemberRepository.CreateAsync(eventScheduleStaffMember);
	}

	public async Task DeleteAsync(
		Guid companyId,
		Guid id)
		=> await _eventScheduleStaffMemberRepository.DeleteByIdAsync(companyId, id);

	public async Task<bool> ExistsByIdAsync(
		Guid companyId,
		Guid id)
		=> await _eventScheduleStaffMemberRepository.ExistsByIdAsync(companyId, id);

	private async Task ValidateEventScheduleAsync(EventScheduleStaffMember eventScheduleStaffMember)
	{
		Guid eventScheduleId = eventScheduleStaffMember.EventScheduleId;
		Guid companyId = eventScheduleStaffMember.CompanyId;

		EventSchedule? eventSchedule = await _eventScheduleRepository
			.GetByIdAsync(eventScheduleId, companyId);
		if (eventSchedule == null)
			throw new EventScheduleNotFoundException(eventScheduleId);
	}

	private async Task ValidateStaffMemberAsync(EventScheduleStaffMember eventScheduleStaffMember)
	{
		Guid staffMemberId = eventScheduleStaffMember.StaffMemberId;
		Guid companyId = eventScheduleStaffMember.CompanyId;

		StaffMember? eventType = await _staffMemberRepository
			.GetByIdAsync(staffMemberId, companyId);
		if (eventType == null)
			throw new StaffMemberNotFoundException(staffMemberId);
	}
}