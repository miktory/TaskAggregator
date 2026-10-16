using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.ValueObjects;
using Task = System.Threading.Tasks.Task;

namespace TaskAggregator.Domain.Abstractions.Ports;

public interface IRequestRepository
{
    Task<Request?> GetByIdAsync(RequestId id, CancellationToken cancellationToken = default);
    Task<Request?> GetByExternalIdAndRequestSourceIdAsync(
        string externalId,
        RequestSourceId requestSourceId,
        CancellationToken cancellationToken = default);

    Task<bool> CheckIfExistsByExternalIdAndRequestSourceIdAsync(string externalId, RequestSourceId requestSourceId, CancellationToken cancellationToken = default);
    Task AddAsync(Request request, CancellationToken cancellationToken = default);
    Task RemoveAsync(Request request, CancellationToken cancellationToken = default);
}
