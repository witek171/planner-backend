using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class UpdateCompanyBreakTimesRequest
{
	[Required] public int BreakTimeStaff { get; init; }
	[Required] public int BreakTimeParticipants { get; init; }
}