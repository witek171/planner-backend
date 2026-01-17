using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Requests;

public class PaginationRequest
{
	[Range(1, int.MaxValue)] public int Page { get; init; } = 1;
	[Range(1, 500)] public int PageSize { get; init; } = 10;
	[StringLength(100)] public string? Search { get; init; }
}