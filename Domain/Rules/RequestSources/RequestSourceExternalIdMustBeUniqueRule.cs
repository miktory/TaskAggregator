using TaskAggregator.Domain.Abstractions.Base.Rules;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Enums;
using TaskAggregator.Domain.Exceptions;

namespace TaskAggregator.Domain.Rules.RequestSources;

public class RequestSourceExternalIdMustBeUniqueRule : UniquenessRule
{
    private readonly IRequestSourceRepository _repo;
    private readonly string _requestSourceExternalId;
    private static readonly EntityType _entityReference = EntityType.RequestSource;

    internal RequestSourceExternalIdMustBeUniqueRule(IRequestSourceRepository repo, string requestSourceExternalId) : base(_entityReference)
    {
        _repo = repo;
        _requestSourceExternalId = requestSourceExternalId;
    }

    public override async ValueTask CheckAsync(CancellationToken ct = default)
    {
        var exists = await _repo.CheckIfExistsByExternalIdAsync(_requestSourceExternalId, ct);
        if (exists)
            throw new RequestSourceExternalIdMustBeUniqueException(_requestSourceExternalId);
    }
}
