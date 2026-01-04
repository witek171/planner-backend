using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class CompanyConfigResponse
{
	[Required] public Guid CompanyId { get; init; }
	[Required] public int BreakTimeStaff { get; init; }
	[Required] public int BreakTimeParticipants { get; init; }
}