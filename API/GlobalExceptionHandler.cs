using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TaskAggregator.API.Resources.Localization.ErrorMessages;
using TaskAggregator.API.Services;
using TaskAggregator.Shared;

namespace TaskAggregator.API;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetails;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly LocalizationService _localizationSvc;
    private readonly IHostEnvironment _env;

    private record class ValidationErrorObject(string Code, IReadOnlyDictionary<string, object> Args, string? Message);

    public GlobalExceptionHandler(
        IProblemDetailsService problemDetails,
        ILogger<GlobalExceptionHandler> logger,
        IStringLocalizer<ErrorMessagesResource> localizer,
        LocalizationService localizationSvc,
        IHostEnvironment env)
    {
        _problemDetails = problemDetails;
        _logger = logger;
        _env = env;
        _localizationSvc = localizationSvc;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        (int sCode, ProblemDetails details) result = (StatusCodes.Status500InternalServerError, new ProblemDetails() { });
        switch (exception)
        {
            case TaskAggregatorException appEx:
                result = CreateProblemDetailsForTaskAggregatorException(appEx);
                break;
        }

        httpContext.Response.StatusCode = result.sCode;
        _ = await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = result.details,
            Exception = exception

        });

        if (result.sCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        return true;
    }

    private (int statusCode, ProblemDetails details) CreateProblemDetailsForTaskAggregatorException(TaskAggregatorException ex)
    {
        var sCode = StatusCodes.Status500InternalServerError;
        string? message = null;
        string? code = null;
        IReadOnlyDictionary<string, ArgumentValue> sourceArgs = ex.Args ?? new Dictionary<string, ArgumentValue>();

        code = ex.Code;

        var formattedArgs = sourceArgs.ToDictionary(
            x => x.Key,
            x => x.Value?.GetValue());

        switch (ex.Kind)
        {
            case ErrorKind.Validation:
                sCode = StatusCodes.Status400BadRequest;
                break;

            case ErrorKind.RestrictedOperation:
                sCode = StatusCodes.Status403Forbidden;
                break;

            case ErrorKind.UnprocessableState:
                sCode = StatusCodes.Status422UnprocessableEntity;
                break;

            case ErrorKind.ConflictState:
                sCode = StatusCodes.Status409Conflict;
                break;

            case ErrorKind.RequestedResourceNotExists:
                sCode = StatusCodes.Status404NotFound;
                break;


        }

        // Try Get Error Message
        var isLocalizationSuccess = _localizationSvc.TryGetLocalizedMessage(code, sourceArgs, out string? formattedMessage);
        if (isLocalizationSuccess)
            message = formattedMessage;

        // format result output
        var details = new ProblemDetails
        {
            Status = sCode,
            Detail = message,
        };
        details.Extensions["args"] = formattedArgs;
        details.Extensions["code"] = code;

        return (sCode, details);
    }
}
