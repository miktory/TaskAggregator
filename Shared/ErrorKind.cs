namespace TaskAggregator.Shared;

public enum ErrorKind
{
    Validation,
    RestrictedOperation,
    UnprocessableState,
    ConflictState,
    RequestedResourceNotExists

}
