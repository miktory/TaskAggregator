using Mapster;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;

namespace TaskAggregator.API.Dtos.Responses.Mappings;

public sealed class RequestSourceResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RequestSourceDto, RequestSourceResponse>()
              .Map(dest => dest.ExternalId, src => src.ExternalId)
              .Map(dest => dest.Id, src => src.Id);
    }
}
