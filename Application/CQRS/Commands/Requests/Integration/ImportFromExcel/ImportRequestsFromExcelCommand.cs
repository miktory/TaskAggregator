using Mediator;

namespace TaskAggregator.Application.CQRS.Commands.Requests.Integration.ImportFromExcel;

public sealed record ImportRequestsFromExcelCommand(
    Stream stream
) : ICommand
{

}
