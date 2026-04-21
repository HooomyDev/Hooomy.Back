using FluentValidation;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Complaints.Commands.CreateComplaint;

public class CreateComplaintCommandValidator
    : AbstractValidator<CreateComplaintCommand>
{
    public CreateComplaintCommandValidator()
    {
        RuleFor(v => v.ShortDescription)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(v => v.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(v => v.Type)
            .IsInEnum()
            .NotEqual(ComplaintType.Unknown);
    }
}