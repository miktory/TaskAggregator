using Mediator;

namespace TaskAggregator.Application.CQRS.Commands.RequestSources.Create;

public sealed record CreateRequestSourceCommand(
    string ExternalId
) : ICommand<long>
{

}
