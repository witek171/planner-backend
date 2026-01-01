using AutoMapper;
using Schedule.Contracts.Dtos.Requests;
using Schedule.Contracts.Dtos.Responses;

namespace PlannerNet.Extensions;

public static class PaginationExtensions
{
	public static PagedResponse<TDto> ToPagedResponse<TEntity, TDto>(
		this (List<TEntity> Items, int TotalCount) result,
		PaginationRequest pagination,
		IMapper mapper)
	{
		List<TDto> mappedItems = mapper.Map<List<TDto>>(result.Items);
		return new PagedResponse<TDto>(
			mappedItems,
			pagination.Page,
			pagination.PageSize,
			result.TotalCount);
	}
}