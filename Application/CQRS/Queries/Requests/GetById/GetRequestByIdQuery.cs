using Mediator;
using TaskAggregator.Application.CQRS.Dtos.Requests.Output;

namespace TaskAggregator.Application.CQRS.Queries.Requests.GetById;

public sealed record GetRequestByIdQuery(
    long Id
) : IQuery<RequestDto>
{

}
