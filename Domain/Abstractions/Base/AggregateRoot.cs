namespace TaskAggregator.Domain.Abstractions.Base
{
    public abstract class AggregateRoot<T, TId> : Entity<T, TId> where T : Entity<T, TId>
    {
        public AggregateRoot() { }

    }
}
