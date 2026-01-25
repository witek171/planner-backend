namespace ReservationSystem.Domain.Models;

public class CompanyConfig
{
	public Guid CompanyId { get; private set; }
	public int BreakTimeStaff { get; private set; }
	public int BreakTimeParticipants { get; private set; }

	public CompanyConfig(
		Guid companyId,
		int breakTimeStaff,
		int breakTimeParticipants)
	{
		CompanyId = companyId;
		BreakTimeStaff = breakTimeStaff;
		BreakTimeParticipants = breakTimeParticipants;
	}
}