using TaskAggregator.Domain.Abstractions.Base.ValueObjects;
using TaskAggregator.Domain.Enums;
using TaskAggregator.Shared;

namespace TaskAggregator.Application.Exceptions;

public class ChainedEntityNotFoundByIdException : TaskAggregatorException
{
    private const string _entityTypeArgName = "entityType";
    private const string _entityIdArgName = "entityId";
    internal ChainedEntityNotFoundByIdException(Identifier id, EntityType entityType)
        : base(ErrorCodes.Application.ChainedEntityNotFoundById, ErrorKind.RequestedResourceNotExists,
            new Dictionary<string, ArgumentValue>() { { _entityTypeArgName, new ArgumentValue(Enum.GetName<EntityType>(entityType)) }, { _entityIdArgName, new ArgumentValue(id.GetValue()) } })
    {
    }

}
