namespace TaskAggregator.Application.ExternalSystems.Excel;

public sealed record ExcelCell(object? Value, Type? CellType, long RowNum, long ColNum);
