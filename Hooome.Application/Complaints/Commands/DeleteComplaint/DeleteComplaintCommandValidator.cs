using FluentValidation;

namespace Hooome.Application.Complaints.Commands.DeleteComplaint;

public class DeleteComplaintCommandValidator
    : AbstractValidator<DeleteComplaintCommand>
{
    public DeleteComplaintCommandValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
    }
}
