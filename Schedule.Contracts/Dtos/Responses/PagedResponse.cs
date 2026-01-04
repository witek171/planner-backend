using System.ComponentModel.DataAnnotations;

namespace Schedule.Contracts.Dtos.Responses;

public class PagedResponse<T>
{
	[Required] public List<T> Items { get; }
	[Required] public int Page { get; }
	[Required] public int PageSize { get; }
	[Required] public int TotalCount { get; }
	[Required] public int TotalPages { get; }

	public PagedResponse(
		List<T> items,
		int page,
		int pageSize,
		int totalCount)
	{
		Items = items;
		Page = page;
		PageSize = pageSize;
		TotalCount = totalCount;
		TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
	}
}