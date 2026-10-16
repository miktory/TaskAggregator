using Mediator;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.CQRS.Dtos;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;

namespace TaskAggregator.Application.CQRS.Queries.RequestSources.GetFiltered;


public sealed class GetRequestSourceFilteredQueryHandler
    : IQueryHandler<GetRequestSourceFilteredQuery, GenericOffsetPagedResultDto<RequestSourceDto>>
{
    private readonly IRequestSourceQueries _queries;

    public GetRequestSourceFilteredQueryHandler(
        IRequestSourceQueries queries)
        => (_queries) = (queries);

    public async ValueTask<GenericOffsetPagedResultDto<RequestSourceDto>> Handle(GetRequestSourceFilteredQuery query, CancellationToken cancellationToken)
    {
        return await _queries.GetFilteredAsync(query.PagingParams, cancellationToken);
    }
}
