using Mediator;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Services;

namespace TaskAggregator.Application.CQRS.Commands.RequestSources.Create;

public sealed class CreateRequestSourceCommandHandler
    : ICommandHandler<CreateRequestSourceCommand, long>
{
    private readonly IUnitOfWork _uow;
    private readonly RequestSourceManagementDomainService _domainSvc;
    private readonly IRequestSourceRepository _repo;

    public CreateRequestSourceCommandHandler(
        IRequestSourceRepository repo,
        IUnitOfWork unitOfWork,
        RequestSourceManagementDomainService domainService)
        => (_repo, _uow, _domainSvc) = (repo, unitOfWork, domainService);

    public async ValueTask<long> Handle(CreateRequestSourceCommand command, CancellationToken cancellationToken)
    {
        var reqSourceCode = command.ExternalId;
        var entity = await _domainSvc.CreateRequestSourceAsync(reqSourceCode, cancellationToken);
        await _repo.AddAsync(requestSource: entity, cancellationToken: cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken: cancellationToken);
        return entity.Id.Value;
    }
}
