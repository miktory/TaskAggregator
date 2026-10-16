using TaskAggregator.Application.Abstractions.Base;

namespace TaskAggregator.Application.CQRS.Dtos.Requests.Output;

public sealed record RequestDto(long Id, string ExternalId,
    long RequestSourceId, string? Name, string? Description) : IEntityDto
{

}
