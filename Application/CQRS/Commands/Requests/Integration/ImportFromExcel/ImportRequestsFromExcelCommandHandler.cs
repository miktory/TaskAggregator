using System.Globalization;
using Mediator;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.Exceptions;
using TaskAggregator.Application.ExternalSystems.Excel;
using TaskAggregator.Domain.Abstractions.Ports;
using TaskAggregator.Domain.Aggregates;
using TaskAggregator.Domain.Enums;
using TaskAggregator.Domain.Services;

namespace TaskAggregator.Application.CQRS.Commands.Requests.Integration.ImportFromExcel;

public sealed class ImportRequestsFromExcelCommandHandler
    : ICommandHandler<ImportRequestsFromExcelCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IRequestSourceRepository _rsRepo;
    private readonly IRequestRepository _rRepo;
    private readonly RequestManagementDomainService _dSvc;
    private readonly IExcelReader _excelReader;

    public ImportRequestsFromExcelCommandHandler(
        IRequestSourceRepository rsRepo,
        IRequestRepository rRepo,
        IUnitOfWork unitOfWork,
        RequestManagementDomainService dSvc,
        IExcelReader excelReader)
        => (_rsRepo, _rRepo, _uow, _excelReader, _dSvc) = (rsRepo, rRepo, unitOfWork, excelReader, dSvc);

    private record ColumnFormat(ColumnType Type, bool ValueRequired, string ColumnName, long ColNum);

    private enum ColumnType
    {
        ExternalId,
        RequestSourceExternalId,
        Name,
        Description

    }

    public async ValueTask<Unit> Handle(ImportRequestsFromExcelCommand command, CancellationToken cancellationToken)
    {
        var culture = CultureInfo.InvariantCulture;

        var headersParsed = false;
        ExcelCell[] headersActual = Array.Empty<ExcelCell>();

        var columnInfo = new List<ColumnFormat>()
        {
            new ColumnFormat(ColumnType.RequestSourceExternalId, true, "RequestSourceExternalId", 0),
            new ColumnFormat(ColumnType.ExternalId, true, "ExternalId", 1),
            new ColumnFormat(ColumnType.Name, false, "Name", 2),
            new ColumnFormat(ColumnType.Description, false, "Description", 3),
        };
        var colInfoByColNumDict = columnInfo.ToDictionary(c => c.ColNum);
        var colInfoByColType = columnInfo.ToDictionary(c => c.Type);

        var rowNum = 0;
        await foreach (var row in _excelReader.ReadAsync(command.stream, cancellationToken))
        {
            var currRow = rowNum++;
            if (!headersParsed)
            {
                headersActual = row.Select(c => c).ToArray();
                headersParsed = true;
                var headersDict = row.ToDictionary(r => r.ColNum);

                foreach (var kvp in colInfoByColNumDict)
                {
                    var colInfo = kvp.Value;
                    headersDict.TryGetValue(colInfo.ColNum, out var actualHeaderCell);
                    var actualHeaderStr = Convert.ToString(actualHeaderCell?.Value, culture);
                    if (actualHeaderStr != colInfo.ColumnName)
                        throw new ExcelColumnValueIsInvalidException(colInfo.ColNum, currRow);
                }

                continue;
            }

            var rowValues = row.Select(c => c).ToArray();
            var res = new Dictionary<ColumnType, string?>();
            foreach (var val in rowValues)
            {
                colInfoByColNumDict.TryGetValue(val.ColNum, out var currColInfo);
                var strValue = Convert.ToString(val.Value, culture);
                switch (currColInfo?.Type)
                {
                    case ColumnType.ExternalId:
                        res[ColumnType.ExternalId] = strValue;
                        break;

                    case ColumnType.RequestSourceExternalId:
                        res[ColumnType.RequestSourceExternalId] = strValue;
                        break;

                    case ColumnType.Name:
                        res[ColumnType.Name] = string.IsNullOrEmpty(strValue) ? null : strValue;
                        break;

                    case ColumnType.Description:
                        res[ColumnType.Description] = string.IsNullOrEmpty(strValue) ? null : strValue;
                        break;

                    default:
                        break;

                }
            }

            foreach (var kvp in res)
            {
                var colType = kvp.Key;
                var strVal = kvp.Value;
                var colInfo = colInfoByColType[colType];
                if (colInfo.ValueRequired && string.IsNullOrEmpty(strVal))
                    throw new ExcelColumnValueIsInvalidException(colInfo.ColNum, currRow);
            }

            var reqSourceExtId = res[ColumnType.RequestSourceExternalId] ?? string.Empty;
            var extId = res[ColumnType.ExternalId] ?? string.Empty;
            var name = res[ColumnType.Name];
            var description = res[ColumnType.Description];

            var requestSource = await _rsRepo.GetByExternalIdAsync(reqSourceExtId, cancellationToken);
            if (requestSource is null)
                throw new ReferencedEntityNotFoundByExternalIdException(reqSourceExtId, EntityType.RequestSource);

            Request? request = await _rRepo.GetByExternalIdAndRequestSourceIdAsync(extId, requestSource.Id, cancellationToken);

            await using (var transaction = await _uow.BeginTransactionAsync(cancellationToken: cancellationToken))
            {
                // Если запрос существует - обновляем его           
                if (request is not null)
                {
                    request.ChangeName(name);
                    request.ChangeDescription(description);
                }
                else
                {
                    var entity = await _dSvc.CreateRequestAsync(requestSource.Id, name, description, extId, cancellationToken);
                    await _rRepo.AddAsync(request: entity, cancellationToken: cancellationToken);
                }

                await _uow.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }

        }

        return Unit.Value;
    }
}
