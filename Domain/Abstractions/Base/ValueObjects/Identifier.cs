namespace TaskAggregator.Domain.Abstractions.Base.ValueObjects;

public abstract record Identifier
{
    public long Value { get; init; }

    protected Identifier(long value) => Value = value;

    public long GetValue() => Value;

}
