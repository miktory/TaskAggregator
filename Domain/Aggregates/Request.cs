using TaskAggregator.Domain.Abstractions.Base;
using TaskAggregator.Domain.Enums;
using TaskAggregator.Domain.ValueObjects;

namespace TaskAggregator.Domain.Aggregates;

public class Request : AggregateRoot<Request, RequestId>
{
    public override EntityType Kind => EntityType.Request;
    public RequestSourceId RequestSourceId { get; private set; } = default!;
    public string ExternalId { get; private set; } = default!;
    public string? Name { get; private set; }
    public string? Description { get; private set; }

    private Request()
    {

    }

    public void ChangeName(string? newName)
    {
        Name = newName;
    }

    public void ChangeDescription(string? newDescription)
    {
        Description = newDescription;
    }

    internal Request(
        RequestSourceId requestSourceId,
        string externalId,
        string? name,
        string? description)
    {
        RequestSourceId = requestSourceId;
        ExternalId = externalId;
        Name = name;
        Description = description;
    }
}
