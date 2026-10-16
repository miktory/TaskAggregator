using TaskAggregator.Domain.Abstractions.Base.ValueObjects;

namespace TaskAggregator.Domain.ValueObjects;

public sealed record RequestSourceId : Identifier
{
    public RequestSourceId(long value) : base(value) { }
}

