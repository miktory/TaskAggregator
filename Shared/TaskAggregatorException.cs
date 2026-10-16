namespace TaskAggregator.Shared;

public abstract class TaskAggregatorException : Exception
{
    private readonly Dictionary<string, ArgumentValue> _args = new Dictionary<string, ArgumentValue>() { };
    public readonly string Code;        // обязательный код
    public readonly ErrorKind Kind;      // категория 
    public IReadOnlyDictionary<string, ArgumentValue> Args { get => _args; }

    protected TaskAggregatorException(string code, ErrorKind kind,
        IReadOnlyDictionary<string, ArgumentValue> args,
        string? message = null, Exception? inner = null)
        : base(message, inner)
    {
        Code = code; Kind = kind;
        _args = args.ToDictionary();
    }
}
