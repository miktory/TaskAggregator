using TaskAggregator.API.Abstractions.Base;

namespace TaskAggregator.API.Dtos.Responses;

public sealed record GenericOffsetPagedResponse<T>(
    IReadOnlyList<T> Entities,
    long TotalCount,
    int PageSize,
    int PageNumber,
    bool HasNextPage,
    long TotalPages) : IResponse where T : IResponse
{

}
