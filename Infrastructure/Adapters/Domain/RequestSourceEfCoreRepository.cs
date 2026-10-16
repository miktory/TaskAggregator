using Microsoft.EntityFrameworkCore;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.ValueObjects;
using TaskAggregator.Infrastructure.EfCore;
using Task = System.Threading.Tasks.Task;

namespace TaskAggregator.Infrastructure.Adapters.Domain;

public sealed class RequestSourceEfCoreRepository : IRequestSourceRepository
{
    private readonly ApplicationDbContext _context;

    public RequestSourceEfCoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RequestSource entity, CancellationToken cancellationToken = default)
    {
        await _context.RequestSources.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> CheckIfExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _context.RequestSources.AnyAsync(ts => ts.ExternalId == externalId, cancellationToken);
    }

    public async Task<bool> CheckIfExistsByIdAsync(RequestSourceId id, CancellationToken cancellationToken = default)
    {
        return await _context.RequestSources.AnyAsync(rs => rs.Id == id, cancellationToken);
    }

    public async Task<RequestSource?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _context.RequestSources.FirstOrDefaultAsync(ts => ts.ExternalId == externalId, cancellationToken);
    }

    public async Task<RequestSource?> GetByIdAsync(RequestSourceId id, CancellationToken cancellationToken = default)
    {
        return await _context.RequestSources.FirstOrDefaultAsync(ts => ts.Id == id, cancellationToken);
    }

    public Task RemoveAsync(RequestSource entity, CancellationToken cancellationToken = default)
    {
        _context.RequestSources.Remove(entity);
        return Task.CompletedTask;
    }

}
