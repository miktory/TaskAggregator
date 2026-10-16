using System.Data;
using Microsoft.EntityFrameworkCore;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.Exceptions;
using TaskAggregator.Infrastructure.EfCore;

namespace TaskAggregator.Infrastructure.Adapters.Application;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public EfUnitOfWork(ApplicationDbContext context)
        => (_context) = (context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken: cancellationToken);
            return result;
        }
        catch (DbUpdateException ex)
        {
            throw new BaseDataPersistenceException(ex);
        }
    }

    public async Task<ITransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database
            .BeginTransactionAsync(isolationLevel, cancellationToken);
        return new EfTransaction(transaction);
    }
}
