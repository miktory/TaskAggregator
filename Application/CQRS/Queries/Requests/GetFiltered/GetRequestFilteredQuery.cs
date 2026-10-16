using Mediator;
using TaskAggregator.Application.CQRS.Dtos;
using TaskAggregator.Application.CQRS.Dtos.Requests.Output;
using TaskAggregator.Application.CQRS.Params;

namespace TaskAggregator.Application.CQRS.Queries.Requests.GetFiltered;

public sealed record GetRequestFilteredQuery(
    OffsetPagingParams PagingParams
) : IQuery<GenericOffsetPagedResultDto<RequestDto>>
{

}
