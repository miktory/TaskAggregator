using TaskAggregator.Application.Exceptions;

namespace TaskAggregator.Application.CQRS.Params;

public sealed record OffsetPagingParams
{
    public const int MAX_PAGE_SIZE = 1000;
    public const int MIN_PAGE_SIZE = 1;
    public const int MIN_PAGE_NUMBER = 1;
    public const int MAX_DEPTH = int.MaxValue;
    public int PageSize { get; }
    public int PageNumber { get; }


    public OffsetPagingParams(int pageSize, int pageNumber)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;

        if (pageSize < MIN_PAGE_SIZE || pageNumber < MIN_PAGE_NUMBER || pageSize > MAX_PAGE_SIZE)
            throw new InvalidOffsetPagingParamsException(this);

        // проверяем, что skip не выходит за пределы int 
        try
        {
            _ = checked((PageNumber - 1) * PageSize);
        }
        catch (OverflowException)
        {
            throw new InvalidOffsetPagingParamsException(this);
        }

    }

    public int GetSkip()
    {
        if (PageNumber > MIN_PAGE_NUMBER)
            return (PageNumber - 1) * PageSize;
        else return 0;
    }

    public int GetTake() => PageSize;

}
