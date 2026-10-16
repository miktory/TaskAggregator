using Microsoft.EntityFrameworkCore;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.CQRS.Dtos;
using TaskAggregator.Application.CQRS.Dtos.Requests.Output;
using TaskAggregator.Application.CQRS.Params;
using TaskAggregator.Application.Mappers;
using TaskAggregator.Domain.ValueObjects;
using TaskAggregator.Infrastructure.EfCore;

namespace TaskAggregator.Infrastructure.Adapters.Application;

public sealed class RequestEfQueries : IRequestQueries
{
    private readonly ApplicationDbContext _context;

    public RequestEfQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestDto?> GetByIdAsync(RequestId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Requests
            .AsNoTracking()
            .FirstOrDefaultAsync(ts => ts.Id == id, cancellationToken);

        return entity is null ? null : entity.ToDto();
    }

    public async Task<GenericOffsetPagedResultDto<RequestDto>> GetFilteredAsync(OffsetPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        var query = _context.Requests.AsNoTracking();
        var totalCount = await query.LongCountAsync(cancellationToken);

        var skip = pagingParams.GetSkip();
        var take = pagingParams.GetTake();
        var pageSize = pagingParams.PageSize;
        var pageNum = pagingParams.PageNumber;

        var entities = await query
            .OrderBy(rs => rs.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        var dtos = entities.Select(x => x.ToDto()).ToList();

        return new GenericOffsetPagedResultDto<RequestDto>(
            dtos,
            totalCount,
            pageSize,
            pageNum);
    }
}
