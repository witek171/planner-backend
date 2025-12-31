using AutoMapper;
using Schedule.Contracts.Dtos.Responses;

namespace PlannerNet.Converters;

public class PagedResponseConverter<TSource, TDestination>
	: ITypeConverter<PagedResponse<TSource>, PagedResponse<TDestination>>
{
	public PagedResponse<TDestination> Convert(
		PagedResponse<TSource> source,
		PagedResponse<TDestination> destination,
		ResolutionContext context)
	{
		List<TDestination> items = context.Mapper.Map<List<TDestination>>(source.Items);
		return new PagedResponse<TDestination>(
			items,
			source.Page,
			source.PageSize,
			source.TotalCount);
	}
}