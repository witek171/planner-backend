namespace ReservationSystem.Domain.Models;

public class StaffMemberAvailability
{
	public Guid Id { get; }
	public Guid CompanyId { get; private set; }
	public Guid StaffMemberId { get; private set; }
	public DateOnly Date { get; private set; }
	public DateTime StartTime { get; private set; }
	public DateTime EndTime { get; private set; }
	public bool IsAvailable { get; private set; }

	public StaffMemberAvailability()
	{
	}

	public StaffMemberAvailability(
		Guid id,
		Guid companyId,
		Guid staffMemberId,
		DateOnly date,
		DateTime startTime,
		DateTime endTime,
		bool isAvailable)
	{
		Id = id;
		CompanyId = companyId;
		StaffMemberId = staffMemberId;
		Date = date;
		StartTime = startTime;
		EndTime = endTime;
		IsAvailable = isAvailable;
	}

	public void SetCompanyId(Guid companyId)
	{
		if (CompanyId != Guid.Empty)
			throw new InvalidOperationException(
				$"CompanyId is already set to {CompanyId} and cannot be changed");

		CompanyId = companyId;
	}

	public void SetStaffMemberId(Guid staffMemberId)
	{
		if (StaffMemberId != Guid.Empty)
			throw new InvalidOperationException(
				$"StaffMemberId is already set to {StaffMemberId} and cannot be changed");

		StaffMemberId = staffMemberId;
	}

	public void SoftDelete()
	{
		if (!IsAvailable)
			throw new InvalidOperationException(
				$"Availability {Id} for staff member {StaffMemberId} is already marked as unavailable " +
				$"on {Date:yyyy-MM-dd} from {StartTime:HH:mm} to {EndTime:HH:mm} " +
				$"and cannot be changed");

		IsAvailable = false;
	}

	public void MarkAsAvailable()
	{
		if (IsAvailable)
			throw new InvalidOperationException(
				$"Availability {Id} for staff member {StaffMemberId} is already marked as available " +
				$"on {Date:yyyy-MM-dd} from {StartTime:HH:mm} to {EndTime:HH:mm} " +
				$"and cannot be changed");

		IsAvailable = true;
	}
}