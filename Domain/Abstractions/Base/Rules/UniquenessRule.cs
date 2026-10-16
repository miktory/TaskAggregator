using TaskAggregator.Domain.Enums;

namespace TaskAggregator.Domain.Abstractions.Base.Rules;

public abstract class UniquenessRule : Rule
{
    public readonly EntityType EntityReference;

    public UniquenessRule(EntityType entityRef)
    {
        EntityReference = entityRef;
    }

}
