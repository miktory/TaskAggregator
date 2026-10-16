using TaskAggregator.Application.CQRS.Dtos.Requests.Output;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;
using TaskAggregator.Domain.Aggregates;

namespace TaskAggregator.Application.Mappers;

public static class DomainObjectDtoMapper
{
    public static RequestSourceDto ToDto(this RequestSource rs) =>
            new(rs.Id.GetValue(), rs.ExternalId);

    public static RequestDto ToDto(this Request r) =>
        new(r.Id.GetValue(), r.ExternalId, r.RequestSourceId.GetValue(), r.Name, r.Description);

}
