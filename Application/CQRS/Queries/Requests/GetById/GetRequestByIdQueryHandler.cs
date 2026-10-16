using Mediator;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.CQRS.Dtos.Requests.Output;
using TaskAggregator.Application.Exceptions;
using TaskAggregator.Domain.Enums;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Application.CQRS.Queries.Requests.GetById;


public sealed class GetRequestByIdQueryHandler
    : IQueryHandler<GetRequestByIdQuery, RequestDto>
{
    private readonly IRequestQueries _queries;

    public GetRequestByIdQueryHandler(
        IRequestQueries queries)
        => (_queries) = (queries);

    public async ValueTask<RequestDto> Handle(GetRequestByIdQuery query, CancellationToken cancellationToken)
    {
        var idVo = new RequestId(query.Id);
        var res = await _queries.GetByIdAsync(idVo, cancellationToken);
        if (res is null)
            throw new ChainedEntityNotFoundByIdException(idVo, EntityType.Request);

        return res;
    }

}
