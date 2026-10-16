using TaskAggregator.Application.CQRS.Params;
using TaskAggregator.Application.Exceptions;
using TaskAggregator.Shared;
using Xunit;
using static TaskAggregator.Shared.ArgumentValue;

namespace TaskAggregator.Tests.Unit.Application.Params;

public class OffsetPagingParamsTests
{
    public static string[] ExpectedExceptionArgumentNames = ["pageSize", "pageNumber", "maxPageSizeAllowed", "minPageSizeAllowed", "minPageNumberAllowed", "maxDepthAllowed"];

    [Theory]
    [InlineData(OffsetPagingParams.MIN_PAGE_SIZE - 1, OffsetPagingParams.MIN_PAGE_NUMBER)] // pageSize < MIN
    [InlineData(OffsetPagingParams.MIN_PAGE_SIZE, OffsetPagingParams.MIN_PAGE_NUMBER - 1)]      // pageNumber < MIN
    [InlineData(OffsetPagingParams.MAX_PAGE_SIZE + 1, OffsetPagingParams.MIN_PAGE_NUMBER)]   // pageSize > MAX
    public void Constructor_InvalidParams_Throws(int pageSize, int pageNumber)
    {
        var ex = Assert.Throws<InvalidOffsetPagingParamsException>(
            () => new OffsetPagingParams(pageSize, pageNumber));

        CheckException(ex, pageSize, pageNumber);

    }

    [Fact]
    public void GetSkipAndGetTake_WithNormalParams_ReturnsCorrectValues()
    {
        var pSize = OffsetPagingParams.MIN_PAGE_SIZE;
        var pNumber = OffsetPagingParams.MIN_PAGE_NUMBER;

        var p = new OffsetPagingParams(pageSize: pSize + 1, pageNumber: pNumber + 1);

        Assert.Equal(pSize + 1, p.GetSkip());
        Assert.Equal(pSize + 1, p.GetTake());

        // First Page always return skip = 0
        p = new OffsetPagingParams(pageSize: pSize, pageNumber: pNumber);
        Assert.Equal(0, p.GetSkip());
    }


    private void CheckException(InvalidOffsetPagingParamsException ex, int pageSize, int pageNumber)
    {
        // Check ErrorKind and Code
        Assert.Equal(ErrorCodes.Application.InvalidOffsetPagingParametersError, ex.Code);
        Assert.Equal(ErrorKind.Validation, ex.Kind);

        // Check that the argument values ​​match the expected ones.
        //// pageSize
        Assert.Equal(ArgType.@long, ex.Args["pageSize"].ArgumentType);
        Assert.Equal(pageSize, Assert.IsType<long>(ex.Args["pageSize"].GetValue()));
        //// pageNumber
        Assert.Equal(ArgType.@long, ex.Args["pageNumber"].ArgumentType);
        Assert.Equal(pageNumber, Assert.IsType<long>(ex.Args["pageNumber"].GetValue()));
        //// maxPageSizeAllowed
        Assert.Equal(ArgType.@long, ex.Args["maxPageSizeAllowed"].ArgumentType);
        Assert.Equal(OffsetPagingParams.MAX_PAGE_SIZE, Assert.IsType<long>(ex.Args["maxPageSizeAllowed"].GetValue()));
        //// minPageSizeAllowed
        Assert.Equal(ArgType.@long, ex.Args["minPageSizeAllowed"].ArgumentType);
        Assert.Equal(OffsetPagingParams.MIN_PAGE_SIZE, Assert.IsType<long>(ex.Args["minPageSizeAllowed"].GetValue()));
        //// maxDepthAllowed
        Assert.Equal(ArgType.@long, ex.Args["maxDepthAllowed"].ArgumentType);
        Assert.Equal(OffsetPagingParams.MAX_DEPTH, Assert.IsType<long>(ex.Args["maxDepthAllowed"].GetValue()));
        //// minPageNumberAllowed
        Assert.Equal(ArgType.@long, ex.Args["minPageNumberAllowed"].ArgumentType);
        Assert.Equal(OffsetPagingParams.MIN_PAGE_NUMBER, Assert.IsType<long>(ex.Args["minPageNumberAllowed"].GetValue()));

        // Check that the argument names match the expected ones
        var argNames = ex.Args.Select(x => x.Key);
        Assert.Equal(ExpectedExceptionArgumentNames.OrderBy(k => k, StringComparer.Ordinal),
            argNames.OrderBy(k => k, StringComparer.Ordinal));
    }
}
