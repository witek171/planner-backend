namespace Schedule.Contracts.Dtos.Responses;

public class PagedResponse<T>
{
	public List<T> Items { get; private set; } = new();
	public int Page { get; }
	public int PageSize { get; private set; }
	public int TotalCount { get; private set; }
	public int TotalPages { get; }

	public PagedResponse()
	{
	}

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