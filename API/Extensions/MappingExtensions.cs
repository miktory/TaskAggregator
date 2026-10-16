using Mapster;
using TaskAggregator.API.Abstractions.Base;
using TaskAggregator.API.Dtos.Responses;
using TaskAggregator.Application.Abstractions.Base;
using TaskAggregator.Application.CQRS.Dtos;

namespace TaskAggregator.API.Extensions;

public static class MappingExtensions
{
    public static GenericOffsetPagedResponse<TOut> MapToOffsetPagedResponse<TIn, TOut>(this GenericOffsetPagedResultDto<TIn> appLayerDto) where TIn : IEntityDto where TOut : IResponse
    {
        return new GenericOffsetPagedResponse<TOut>(appLayerDto.Entities.Adapt<IReadOnlyList<TOut>>(),
            appLayerDto.TotalCount, appLayerDto.PageSize, appLayerDto.PageNumber, appLayerDto.HasNextPage, appLayerDto.TotalPages);
    }
}
