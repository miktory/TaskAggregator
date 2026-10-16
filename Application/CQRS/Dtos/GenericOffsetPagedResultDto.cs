using TaskAggregator.Application.Abstractions.Base;

namespace TaskAggregator.Application.CQRS.Dtos;

public sealed record GenericOffsetPagedResultDto<T>(
    IReadOnlyList<T> Entities,
    long TotalCount,
    int PageSize,
    int PageNumber) where T : IEntityDto
{
    public long TotalPages => PageSize <= 0
        ? 0
        : (TotalCount + PageSize - 1) / PageSize;

    public bool HasNextPage => PageNumber < TotalPages;
}
