using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Requests;

public class SpecializationRequest
{
	[Required] public string Name { get; init; }
	[Required] public string Description { get; init; }
}