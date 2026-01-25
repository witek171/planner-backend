using AutoMapper;
using ReservationSystem.Contracts.Dtos.Requests;
using ReservationSystem.Contracts.Dtos.Responses;

namespace ReservationSystem.Extensions;

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