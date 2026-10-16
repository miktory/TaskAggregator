using TaskAggregator.Application.CQRS.Dtos;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;
using TaskAggregator.Application.CQRS.Params;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Application.Abstractions.Ports;

public interface IRequestSourceQueries
{
    public Task<RequestSourceDto?> GetByIdAsync(RequestSourceId id, CancellationToken cancellationToken = default);

    public Task<GenericOffsetPagedResultDto<RequestSourceDto>> GetFilteredAsync(OffsetPagingParams pagingParams, CancellationToken cancellationToken = default);
}
