using FluentValidation;

namespace Hooome.Application.CQRS.Works.Commands.UpdateWork;

public class UpdateWorkCommandValidator
    : AbstractValidator<UpdateWorkCommand>
{
    public UpdateWorkCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().NotEqual(Guid.Empty);

        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(c => c.Category)
            .IsInEnum();

        RuleFor(c => c.Seriousness)
            .IsInEnum();

        RuleFor(c => c.PlannedStartTime)
            .LessThan(c => c.PlannedEndTime);

        RuleFor(c => c.PlannedEndTime)
            .GreaterThan(c => c.PlannedStartTime);
    }
}
