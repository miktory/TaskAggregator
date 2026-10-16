using TaskAggregator.Domain.ValueObjects;
using TaskAggregator.Shared;

namespace TaskAggregator.Domain.Exceptions;

public class RequestExternalIdMustBeUniqueWithinRequestSourceIdException : TaskAggregatorException
{
    private const string _reqExtIdArgName = "externalId";
    private const string _reqSourceIdArgName = "requestSourceId";
    internal RequestExternalIdMustBeUniqueWithinRequestSourceIdException(string requestExternalId, RequestSourceId requestSourceId)
        : base(ErrorCodes.Domain.RequestExternalIdMustBeUniqueWithinRequestSourceId, ErrorKind.ConflictState,
            new Dictionary<string, ArgumentValue>() { { _reqExtIdArgName, new ArgumentValue(requestExternalId) }, { _reqSourceIdArgName, new ArgumentValue(requestSourceId.GetValue()) } })
    {

    }

}
