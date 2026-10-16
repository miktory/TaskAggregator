using Mediator;
using TaskAggregator.Application.CQRS.Dtos;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;
using TaskAggregator.Application.CQRS.Params;

namespace TaskAggregator.Application.CQRS.Queries.RequestSources.GetFiltered;

public sealed record GetRequestSourceFilteredQuery(
    OffsetPagingParams PagingParams
) : IQuery<GenericOffsetPagedResultDto<RequestSourceDto>>
{

}
