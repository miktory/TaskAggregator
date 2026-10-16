using TaskAggregator.Application.CQRS.Params;
using TaskAggregator.Shared;

namespace TaskAggregator.Application.Exceptions;

public class InvalidOffsetPagingParamsException : TaskAggregatorException
{
    private const string _pageNumMinAllowedArgName = "minPageNumberAllowed";
    private const string _pageNumArgName = "pageNumber";
    private const string _pageSizeMinAllowedArgName = "minPageSizeAllowed";
    private const string _pageSizeMaxAllowedArgName = "maxPageSizeAllowed";
    private const string _pageSizeArgName = "pageSize";
    private const string _maxDepthArgName = "maxDepthAllowed";

    internal InvalidOffsetPagingParamsException(OffsetPagingParams pagingParams)
        : base(ErrorCodes.Application.InvalidOffsetPagingParametersError, ErrorKind.Validation,
            new Dictionary<string, ArgumentValue>() {{_pageNumMinAllowedArgName, new ArgumentValue(OffsetPagingParams.MIN_PAGE_NUMBER)}, {_pageNumArgName, new ArgumentValue(pagingParams.PageNumber)},
            {_pageSizeMinAllowedArgName, new ArgumentValue(OffsetPagingParams.MIN_PAGE_SIZE)}, {_pageSizeMaxAllowedArgName, new ArgumentValue(OffsetPagingParams.MAX_PAGE_SIZE)},
            {_pageSizeArgName, new ArgumentValue(pagingParams.PageSize)}, {_maxDepthArgName, new ArgumentValue(OffsetPagingParams.MAX_DEPTH)}})
    {
    }

}
