using System.Runtime.CompilerServices;
using ExcelDataReader;
using ExcelDataReader.Exceptions;
using TaskAggregator.Application.Abstractions.Ports;
using TaskAggregator.Application.Exceptions;
using TaskAggregator.Application.ExternalSystems.Excel;

namespace TaskAggregator.Infrastructure.Adapters.Application;

public sealed class StreamExcelReader : IExcelReader
{
    public async IAsyncEnumerable<IReadOnlyList<ExcelCell>> ReadAsync(
     Stream stream,
     [EnumeratorCancellation] CancellationToken ct = default)
    {
        IExcelDataReader? reader;
        try
        {
            reader = ExcelReaderFactory.CreateReader(stream,
                new ExcelReaderConfiguration { LeaveOpen = true });   // сигнатура-проверка здесь
        }
        catch (ExcelReaderException ex)
        {
            throw new BaseFileProcessingErrorException(ex);
        }

        try
        {
            long rowNum = 0;
            while (reader.Read())
            {
                ct.ThrowIfCancellationRequested();
                var cells = new ExcelCell[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    cells[i] = new ExcelCell(value, value?.GetType(), rowNum, i);
                }
                rowNum++;
                yield return cells;
            }
        }
        finally
        {
            reader?.Dispose();   // stream не трогаем (LeaveOpen=true)
        }
    }
}
