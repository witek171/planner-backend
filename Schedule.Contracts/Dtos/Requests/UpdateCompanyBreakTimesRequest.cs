using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Requests;

public class UpdateCompanyBreakTimesRequest
{
	[Required] public int BreakTimeStaff { get; init; }
	[Required] public int BreakTimeParticipants { get; init; }
}