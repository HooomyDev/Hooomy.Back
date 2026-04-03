using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Commands.DeleteRequest;

public class DeleteRequestCommandValidator 
    : AbstractValidator<DeleteRequestCommand>
{
    public DeleteRequestCommandValidator()
    {
        RuleFor(c => c.UserId).NotEqual(Guid.Empty);
        RuleFor(c => c.Id).NotEqual(Guid.Empty);
    }
}
