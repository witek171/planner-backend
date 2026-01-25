using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Contracts.Dtos.Responses;

public class SpecializationResponse
{
	[Required] public Guid Id { get; init; }
	[Required] public string Name { get; init; }
	[Required] public string Description { get; init; }
}