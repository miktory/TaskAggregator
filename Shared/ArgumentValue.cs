namespace TaskAggregator.Shared;

public sealed class ArgumentValue
{
    public enum ArgType
    {
        @string,
        @long,
        @double,
        @bool
    }
    public ArgType ArgumentType { get; init; }
    public bool IsCollection { get; init; }
    public IReadOnlyList<long?>? LongValues { get; init; }
    public IReadOnlyList<string?>? StrValues { get; init; }
    public IReadOnlyList<double?>? DblValues { get; init; }
    public IReadOnlyList<bool?>? BoolValues { get; init; }

    public ArgumentValue(long? value) => (LongValues, ArgumentType) = ([value], ArgType.@long);
    public ArgumentValue(string? value) => (StrValues, ArgumentType) = ([value], ArgType.@string);
    public ArgumentValue(double? value) => (DblValues, ArgumentType) = ([value], ArgType.@double);
    public ArgumentValue(bool? value) => (BoolValues, ArgumentType) = ([value], ArgType.@bool);

    public ArgumentValue(long?[] values) => (LongValues, ArgumentType, IsCollection) = (values, ArgType.@long, true);
    public ArgumentValue(string?[] values) => (StrValues, ArgumentType, IsCollection) = (values, ArgType.@string, true);
    public ArgumentValue(double?[] values) => (DblValues, ArgumentType, IsCollection) = (values, ArgType.@double, true);
    public ArgumentValue(bool?[] values) => (BoolValues, ArgumentType, IsCollection) = (values, ArgType.@bool, true);

    public object? GetValue()
    {
        IReadOnlyList<object?>? values = ArgumentType switch
        {
            ArgType.@string => StrValues,
            ArgType.@long => LongValues?.Cast<object?>().ToList(),
            ArgType.@double => DblValues?.Cast<object?>().ToList(),
            ArgType.@bool => BoolValues?.Cast<object?>().ToList(),
            _ => throw new NotImplementedException()
        };

        if (values is null)
            return null;

        return IsCollection ? values : values?.First();

    }

}
