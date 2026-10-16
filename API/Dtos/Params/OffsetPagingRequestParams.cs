namespace TaskAggregator.API.Dtos.Params;

public sealed record OffsetPagingRequestParams
{
    public int PageSize { get; } = 50;
    public int PageNumber { get; } = 1;

}
