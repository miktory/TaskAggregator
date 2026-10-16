using TaskAggregator.Shared;

namespace TaskAggregator.Domain.Exceptions;

public class RequestSourceExternalIdMustBeUniqueException : TaskAggregatorException
{
    private const string _reqSourceExtIdArgName = "externalId";
    internal RequestSourceExternalIdMustBeUniqueException(string requestSourceExternalId)
        : base(ErrorCodes.Domain.RequestSourceExternalIdMustBeUnique, ErrorKind.ConflictState,
            new Dictionary<string, ArgumentValue>() { { _reqSourceExtIdArgName, new ArgumentValue(requestSourceExternalId) } })
    {
    }

}
