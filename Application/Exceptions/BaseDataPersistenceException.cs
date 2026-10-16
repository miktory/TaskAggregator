using TaskAggregator.Shared;

namespace TaskAggregator.Application.Exceptions;

public class BaseDataPersistenceException : TaskAggregatorException
{
    public BaseDataPersistenceException(Exception? inner)
        : base(ErrorCodes.Application.BaseDataPersistenceError, ErrorKind.ConflictState,
            new Dictionary<string, ArgumentValue>(), null, inner)
    {
    }

}
