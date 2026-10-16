using Microsoft.EntityFrameworkCore;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.ValueObjects;
using TaskAggregator.Infrastructure.EfCore;
using Task = System.Threading.Tasks.Task;

namespace TaskAggregator.Infrastructure.Adapters.Domain;

public sealed class RequestEfCoreRepository : IRequestRepository
{
    private readonly ApplicationDbContext _context;

    public RequestEfCoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Request?> GetByIdAsync(
        RequestId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Requests
            .FirstOrDefaultAsync(request => request.Id == id, cancellationToken);
    }

    public async Task<Request?> GetByExternalIdAndRequestSourceIdAsync(
        string externalId,
        RequestSourceId requestSourceId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Requests
            .FirstOrDefaultAsync(
                request => request.ExternalId == externalId
                    && request.RequestSourceId == requestSourceId,
                cancellationToken);
    }

    public async Task AddAsync(
        Request request,
        CancellationToken cancellationToken = default)
    {
        await _context.Requests.AddAsync(request, cancellationToken);
    }

    public Task RemoveAsync(
        Request request,
        CancellationToken cancellationToken = default)
    {
        _context.Requests.Remove(request);
        return Task.CompletedTask;
    }

    public async Task<bool> CheckIfExistsByExternalIdAndRequestSourceIdAsync(string externalId, RequestSourceId requestSourceId, CancellationToken cancellationToken = default)
    {
        return await _context.Requests.AnyAsync(r => r.ExternalId == externalId && r.RequestSourceId == requestSourceId, cancellationToken);
    }

}
