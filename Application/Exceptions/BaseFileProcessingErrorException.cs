using TaskAggregator.Shared;

namespace TaskAggregator.Application.Exceptions;

public class BaseFileProcessingErrorException : TaskAggregatorException
{
    public BaseFileProcessingErrorException(Exception? inner)
        : base(ErrorCodes.Application.FileProcessingError, ErrorKind.UnprocessableState,
            new Dictionary<string, ArgumentValue> { }, null, inner)
    {
    }

}
