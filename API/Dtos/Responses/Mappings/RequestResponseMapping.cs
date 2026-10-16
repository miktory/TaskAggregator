using Mapster;
using TaskAggregator.Application.CQRS.Dtos.Requests.Output;

namespace TaskAggregator.API.Dtos.Responses.Mappings;

public sealed class RequestResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RequestDto, RequestResponse>()
              .Map(dest => dest.ExternalId, src => src.ExternalId)
              .Map(dest => dest.Id, src => src.Id)
              .Map(dest => dest.Name, src => src.Name)
              .Map(dest => dest.RequestSourceId, src => src.RequestSourceId)
              .Map(dest => dest.Description, src => src.Description);
    }
}
