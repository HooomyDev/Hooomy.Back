using FluentValidation;

namespace Hooome.Application.CQRS.Works.Commands.CreateWork;

public class CreateWorkCommandValidator : AbstractValidator<CreateWorkCommand>
{
    public CreateWorkCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(c => c.Street)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.House)
            .GreaterThan(0);

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
