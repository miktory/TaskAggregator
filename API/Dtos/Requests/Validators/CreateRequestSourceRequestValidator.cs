using FluentValidation;

namespace TaskAggregator.API.Dtos.Requests.Validators;

public class CreateRequestSourceRequestValidator : AbstractValidator<CreateRequestSourceRequest>
{
    public CreateRequestSourceRequestValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .MaximumLength(36);

    }
}
