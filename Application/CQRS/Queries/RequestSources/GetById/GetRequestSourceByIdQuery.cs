using Mediator;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;

namespace TaskAggregator.Application.CQRS.Queries.RequestSources.GetById;

public sealed record GetRequestSourceByIdQuery(
    long Id
) : IQuery<RequestSourceDto>
{

}
