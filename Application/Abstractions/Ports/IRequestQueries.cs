using TaskAggregator.Application.CQRS.Dtos;
using TaskAggregator.Application.CQRS.Dtos.Requests.Output;
using TaskAggregator.Application.CQRS.Params;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Application.Abstractions.Ports;

public interface IRequestQueries
{
    public Task<RequestDto?> GetByIdAsync(RequestId id, CancellationToken cancellationToken = default);

    public Task<GenericOffsetPagedResultDto<RequestDto>> GetFilteredAsync(OffsetPagingParams pagingParams, CancellationToken cancellationToken = default);
}
