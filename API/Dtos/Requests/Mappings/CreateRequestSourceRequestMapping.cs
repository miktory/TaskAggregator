using Mapster;
using TaskAggregator.Application.CQRS.Commands.RequestSources.Create;

namespace TaskAggregator.API.Dtos.Requests.Mappings;

public sealed class CreateRequestSourceRequestMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateRequestSourceRequest, CreateRequestSourceCommand>()
              .Map(dest => dest.ExternalId, src => src.ExternalId);
    }
}
