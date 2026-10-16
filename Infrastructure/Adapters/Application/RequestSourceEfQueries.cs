using Microsoft.EntityFrameworkCore;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.CQRS.Dtos;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;
using TaskAggregator.Application.CQRS.Params;
using TaskAggregator.Application.Mappers;
using TaskAggregator.Domain.ValueObjects;
using TaskAggregator.Infrastructure.EfCore;

namespace TaskAggregator.Infrastructure.Adapters.Application;

public sealed class RequestSourceEfQueries : IRequestSourceQueries
{
    private readonly ApplicationDbContext _context;

    public RequestSourceEfQueries(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestSourceDto?> GetByIdAsync(RequestSourceId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.RequestSources
            .AsNoTracking()
            .FirstOrDefaultAsync(ts => ts.Id == id, cancellationToken);

        return entity is null ? null : entity.ToDto();
    }

    public async Task<GenericOffsetPagedResultDto<RequestSourceDto>> GetFilteredAsync(OffsetPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        var query = _context.RequestSources.AsNoTracking();
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

        return new GenericOffsetPagedResultDto<RequestSourceDto>(
            dtos,
            totalCount,
            pageSize,
            pageNum);
    }
}
