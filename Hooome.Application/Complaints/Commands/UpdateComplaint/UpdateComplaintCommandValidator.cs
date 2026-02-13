using FluentValidation;
using Hooome.Domain.Enums;

namespace Hooome.Application.Complaints.Commands.UpdateComplaint;

public class UpdateComplaintCommandValidator
    : AbstractValidator<UpdateComplaintCommand>
{
    public UpdateComplaintCommandValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);

        RuleFor(v => v.ShortDescription)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(v => v.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(v => v.Status)
            .IsInEnum()
            .NotEqual(ComplaintStatus.Unknown);
    }
}