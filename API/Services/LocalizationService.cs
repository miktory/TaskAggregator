using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;
using TaskAggregator.API.Resources.Localization.ErrorMessages;
using TaskAggregator.Shared;


namespace TaskAggregator.API.Services;

public class LocalizationService
{
    private readonly IStringLocalizer<ErrorMessagesResource> _appErrorsLocalizer;
    private static readonly Regex PlaceholderPattern = new(@"\{([a-zA-Z0-9]+)\}", RegexOptions.Compiled);
    public LocalizationService(IStringLocalizer<ErrorMessagesResource> appErrorsLocalizer) => (_appErrorsLocalizer) = (appErrorsLocalizer);

    public bool TryGetLocalizedMessage(string code, IReadOnlyDictionary<string, ArgumentValue> args, out string? resultMessage)
    {
        // Format localized message
        var template = _appErrorsLocalizer.GetString(code);
        if (template.ResourceNotFound)
        {
            resultMessage = null;
            return false;
        }

        var isFormatted = TryFormatMessage(template, args, out string formattedMessage, out var missingPlaceholders);
        if (!isFormatted)
        {
            resultMessage = null;
            return false;
        }

        resultMessage = formattedMessage;
        return true;
    }

    private static bool TryFormatMessage(
       string template,
       IReadOnlyDictionary<string, ArgumentValue> data,
       out string result,
       out IReadOnlyCollection<string> missingPlaceholders)
    {
        var missing = new List<string>();

        result = PlaceholderPattern.Replace(template, match =>
        {
            var key = match.Groups[1].Value;

            if (data.TryGetValue(key, out var value))
                return FormatArgumentValue(value);

            if (!missing.Contains(key))
                missing.Add(key);

            return match.Value;
        });

        missingPlaceholders = missing;
        return missing.Count == 0;
    }

    private static string FormatObject(object? value)
        => value switch
        {
            null => "Null",
            string s => s,
            bool b => b.ToString(),
            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture) ?? "Null",
            _ => "Null"
        };

    private static string FormatArgumentValue(ArgumentValue value)
    {
        if (value is null)
            return FormatObject(null);

        var objVal = value.GetValue();
        if (value.IsCollection)
        {
            var itemCol = (objVal as IReadOnlyList<object?>)?.Select(x => FormatObject(x));
            return itemCol is null ? string.Empty : $"[{string.Join(", ", itemCol)}]";
        }
        else
            return FormatObject(objVal);
    }

}
