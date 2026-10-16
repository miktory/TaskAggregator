using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskAggregator.API.ActionFilters.Async;

public sealed class FluentValidationAsyncActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid)
        {
            var services = context.HttpContext.RequestServices;

            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value is null) continue;

                var validators = services
                    .GetServices(typeof(IValidator<>).MakeGenericType(argument.Value.GetType()))
                    .Cast<IValidator>()
                    .ToArray();

                if (validators.Length == 0) continue;

                var validationContext = new ValidationContext<object>(argument.Value);
                var results = await Task.WhenAll(validators.Select(
                    v => v.ValidateAsync(validationContext, context.HttpContext.RequestAborted)));

                var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToArray();
                if (failures.Length > 0)
                {
                    //throw new ValidationException(failures);
                    foreach (var failure in failures)
                        context.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
            }
        }

        await next();
    }
}
