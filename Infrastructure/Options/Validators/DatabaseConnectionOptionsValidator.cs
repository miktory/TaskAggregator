using Microsoft.Extensions.Options;

namespace TaskAggregator.Infrastructure.Options.Validators;

public class DatabaseConnectionOptionsValidator : IValidateOptions<DatabaseConnectionOptions>
{
    public ValidateOptionsResult Validate(string? name, DatabaseConnectionOptions options)
    {
        var failures = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            failures.Add("DatabaseConnection:ConnectionString is required.");


        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}
