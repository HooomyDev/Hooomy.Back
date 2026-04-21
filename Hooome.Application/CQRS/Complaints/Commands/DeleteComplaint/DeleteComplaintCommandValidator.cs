using FluentValidation;

namespace Hooome.Application.CQRS.Complaints.Commands.DeleteComplaint;

public class DeleteComplaintCommandValidator
    : AbstractValidator<DeleteComplaintCommand>
{
    public DeleteComplaintCommandValidator()
    {
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
    }
}
