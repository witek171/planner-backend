using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class CompanyConfigResponse
{
	[Required] public Guid CompanyId { get; }
	[Required] public int BreakTimeStaff { get; }
	[Required] public int BreakTimeParticipants { get; }

	public CompanyConfigResponse(
		Guid companyId,
		int breakTimeStaff,
		int breakTimeParticipants)
	{
		CompanyId = companyId;
		BreakTimeStaff = breakTimeStaff;
		BreakTimeParticipants = breakTimeParticipants;
	}
}