using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.ValueObjects;
using Task = System.Threading.Tasks.Task;

namespace TaskAggregator.Domain.Abstractions.Ports;

public interface IRequestSourceRepository
{
    public Task<RequestSource?> GetByIdAsync(RequestSourceId id, CancellationToken cancellationToken = default);
    public Task<RequestSource?> GetByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
    public Task<bool> CheckIfExistsByExternalIdAsync(string externalId, CancellationToken cancellationToken = default);
    public Task<bool> CheckIfExistsByIdAsync(RequestSourceId id, CancellationToken cancellationToken = default);
    public Task AddAsync(RequestSource requestSource, CancellationToken cancellationToken = default);
    public Task RemoveAsync(RequestSource requestSource, CancellationToken cancellationToken = default);
}
