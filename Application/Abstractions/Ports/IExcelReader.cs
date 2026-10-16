using TaskAggregator.Application.ExternalSystems.Excel;

namespace TaskAggregator.Application.Abstractions.Ports;

public interface IExcelReader
{
    IAsyncEnumerable<IReadOnlyList<ExcelCell>> ReadAsync(Stream stream, CancellationToken ct = default);
}
