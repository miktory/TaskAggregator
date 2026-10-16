using TaskAggregator.Domain.Abstractions.Base;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.Rules.RequestSources;

namespace TaskAggregator.Domain.Services;

public class RequestSourceManagementDomainService
{
    private readonly IRequestSourceRepository _repo;
    public RequestSourceManagementDomainService(IRequestSourceRepository repo)
    {
        _repo = repo;
    }

    public async Task<RequestSource> CreateRequestSourceAsync(string externalId, CancellationToken cancellationToken)
    {
        var rs = new RequestSource(externalId);
        Rule[] rules =
        [
            new RequestSourceExternalIdMustBeUniqueRule(_repo, externalId)
        ];
        foreach (var rule in rules)
            await rs.CheckRuleAsync(rule, cancellationToken);

        return rs;
    }
}
