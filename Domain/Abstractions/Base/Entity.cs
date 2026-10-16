using TaskAggregator.Domain.Enums;

namespace TaskAggregator.Domain.Abstractions.Base;


public abstract class Entity
{
    // Стабильный бизнес-идентификатор типа сущности, не зависящий от имени типа
    public abstract EntityType Kind { get; }
    public async ValueTask CheckRuleAsync(Rule rule, CancellationToken ct = default)
        => await rule.CheckAsync(ct);

}

public abstract class Entity<TId> : Entity            // id-вариант
{
    public TId Id { get; set; } = default!;
}

public abstract class Entity<T, TId> : Entity<TId>    // self-referencing
    where T : Entity<T, TId>
{
}
