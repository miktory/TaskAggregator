using TaskAggregator.API.Abstractions.Base;

namespace TaskAggregator.API.Dtos.Responses;

public sealed record RequestSourceResponse(long Id,
    string ExternalId
) : IResponse
{

}
