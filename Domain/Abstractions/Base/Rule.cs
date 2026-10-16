namespace TaskAggregator.Domain.Abstractions.Base;

public abstract class Rule
{
    public Rule() { }

    public abstract ValueTask CheckAsync(CancellationToken ct = default);


}
