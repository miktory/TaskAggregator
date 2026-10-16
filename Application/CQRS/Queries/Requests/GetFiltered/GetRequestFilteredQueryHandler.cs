using Mediator;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.CQRS.Dtos;
using TaskAggregator.Application.CQRS.Dtos.Requests.Output;

namespace TaskAggregator.Application.CQRS.Queries.Requests.GetFiltered;


public sealed class GetRequestFilteredQueryHandler
    : IQueryHandler<GetRequestFilteredQuery, GenericOffsetPagedResultDto<RequestDto>>
{
    private readonly IRequestQueries _queries;

    public GetRequestFilteredQueryHandler(
        IRequestQueries queries)
        => (_queries) = (queries);

    public async ValueTask<GenericOffsetPagedResultDto<RequestDto>> Handle(GetRequestFilteredQuery query, CancellationToken cancellationToken)
    {
        return await _queries.GetFilteredAsync(query.PagingParams, cancellationToken);
    }
}
