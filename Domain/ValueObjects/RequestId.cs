using TaskAggregator.Domain.Abstractions.Base.ValueObjects;

namespace TaskAggregator.Domain.ValueObjects;

public sealed record RequestId : Identifier
{
    public RequestId(long value) : base(value) { }
}

