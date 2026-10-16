using TaskAggregator.API.Abstractions.Base;

namespace TaskAggregator.API.Dtos.Responses;

public sealed record RequestResponse(long Id, string ExternalId,
    long RequestSourceId, string? Name, string? Description) : IResponse
{

}
