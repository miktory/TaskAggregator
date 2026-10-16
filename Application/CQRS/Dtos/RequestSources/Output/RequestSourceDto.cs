using TaskAggregator.Application.Abstractions.Base;

namespace TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;

public sealed record RequestSourceDto : IEntityDto
{
    public long Id { get; init; }
    public string ExternalId { get; init; }

    public RequestSourceDto(long id, string externalId) => (Id, ExternalId) = (id, externalId);
}
