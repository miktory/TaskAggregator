using TaskAggregator.Domain.Abstractions.Base;
using TaskAggregator.Domain.Enums;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Domain.Aggregates
{
    public class RequestSource : AggregateRoot<RequestSource, RequestSourceId>
    {
        public override EntityType Kind => EntityType.RequestSource;
        public string ExternalId { get; private set; } = default!;

        private RequestSource()
        {

        }
        internal RequestSource(string externalId)
        {
            ExternalId = externalId;
        }
    }
}
