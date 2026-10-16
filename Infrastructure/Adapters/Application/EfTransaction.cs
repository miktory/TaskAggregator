using Microsoft.EntityFrameworkCore.Storage;
using TaskAggregator.Application.Abstractions.Ports;

namespace TaskAggregator.Infrastructure.Adapters.Application;

public sealed class EfTransaction : ITransaction
{
    private readonly IDbContextTransaction _transaction;

    public EfTransaction(IDbContextTransaction transaction)
        => _transaction = transaction;

    public Task CommitAsync(CancellationToken cancellationToken = default)
        => _transaction.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default)
        => _transaction.RollbackAsync(cancellationToken);

    public ValueTask DisposeAsync()
        => _transaction.DisposeAsync();
}
