using TaskAggregator.Domain.Abstractions.Base;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.Rules.Requests;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Domain.Services;

public class RequestManagementDomainService
{
    private readonly IRequestRepository _repo;
    private readonly IRequestSourceRepository _rsRepo;
    public RequestManagementDomainService(IRequestRepository repo, IRequestSourceRepository rsRepo)
    {
        _repo = repo;
        _rsRepo = rsRepo;
    }

    public async Task<Request> CreateRequestAsync(RequestSourceId requestSourceId, string? name, string? description, string externalId, CancellationToken cancellationToken)
    {
        var r = new Request(requestSourceId, externalId, name, description);
        Rule[] rules =
        [
            new RequestExternalIdMustBeUniqueWithinRequestSourceIdRule(_repo, externalId, requestSourceId)
        ];
        foreach (var rule in rules)
            await r.CheckRuleAsync(rule, cancellationToken);

        return r;
    }
}
