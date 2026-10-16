using TaskAggregator.Shared;

namespace TaskAggregator.Application.Exceptions;

public class ExcelColumnValueIsInvalidException : TaskAggregatorException
{
    private const string _rowNumArgName = "rowNumber";
    private const string _columnNumArgName = "columnNumber";

    internal ExcelColumnValueIsInvalidException(long colNum, long rowNum)
        : base(ErrorCodes.Application.ExcelColumnValueIsInvalid, ErrorKind.UnprocessableState,
            new Dictionary<string, ArgumentValue>() { { _columnNumArgName, new ArgumentValue(colNum) }, { _rowNumArgName, new ArgumentValue(rowNum) } })
    {
    }

}
