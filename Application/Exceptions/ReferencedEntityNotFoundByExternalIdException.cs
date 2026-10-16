using TaskAggregator.Domain.Enums;
using TaskAggregator.Shared;

namespace TaskAggregator.Application.Exceptions;

public class ReferencedEntityNotFoundByExternalIdException : TaskAggregatorException
{
    private const string _entityTypeArgName = "entityType";
    private const string _entityExtIdArgName = "externalId";
    internal ReferencedEntityNotFoundByExternalIdException(string externalId, EntityType entityType)
        : base(ErrorCodes.Application.ReferencedEntityNotFoundByExternalId, ErrorKind.UnprocessableState,
            new Dictionary<string, ArgumentValue>() { { _entityTypeArgName, new ArgumentValue(Enum.GetName<EntityType>(entityType)) }, { _entityExtIdArgName, new ArgumentValue(externalId) } })
    {
    }

}
