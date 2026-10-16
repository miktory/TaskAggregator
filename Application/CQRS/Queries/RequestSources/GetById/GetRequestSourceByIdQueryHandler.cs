using Mediator;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;
using TaskAggregator.Application.Exceptions;
using TaskAggregator.Domain.Enums;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Application.CQRS.Queries.RequestSources.GetById;


public sealed class GetRequestSourceByIdQueryHandler
    : IQueryHandler<GetRequestSourceByIdQuery, RequestSourceDto>
{
    private readonly IRequestSourceQueries _queries;

    public GetRequestSourceByIdQueryHandler(
        IRequestSourceQueries queries)
        => (_queries) = (queries);

    public async ValueTask<RequestSourceDto> Handle(GetRequestSourceByIdQuery query, CancellationToken cancellationToken)
    {
        var idVo = new RequestSourceId(query.Id);
        var res = await _queries.GetByIdAsync(idVo, cancellationToken);
        if (res is null)
            throw new ChainedEntityNotFoundByIdException(idVo, EntityType.RequestSource);

        return res;
    }

}
