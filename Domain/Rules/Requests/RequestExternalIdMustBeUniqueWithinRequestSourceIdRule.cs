using TaskAggregator.Domain.Abstractions.Base.Rules;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Enums;
using TaskAggregator.Domain.Exceptions;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Domain.Rules.Requests;

public class RequestExternalIdMustBeUniqueWithinRequestSourceIdRule : UniquenessRule
{
    private readonly IRequestRepository _repo;
    private readonly string _requestExternalId;
    private readonly RequestSourceId _requestSourceId;
    private static readonly EntityType _entityReference = EntityType.Request;

    internal RequestExternalIdMustBeUniqueWithinRequestSourceIdRule(IRequestRepository repo, string requestExternalId, RequestSourceId requestSourceId)
        : base(_entityReference)
    {
        _repo = repo;
        _requestSourceId = requestSourceId;
        _requestExternalId = requestExternalId;
    }

    public override async ValueTask CheckAsync(CancellationToken ct = default)
    {
        var exists = await _repo.CheckIfExistsByExternalIdAndRequestSourceIdAsync(_requestExternalId, _requestSourceId, ct);
        if (exists)
            throw new RequestExternalIdMustBeUniqueWithinRequestSourceIdException(_requestExternalId, _requestSourceId);
    }
}
