namespace TaskAggregator.API.Dtos.Params;

public sealed record OffsetPagingRequestParams
{
    public int PageSize { get; init; } = 50;
    public int PageNumber { get; init; } = 1;

}
