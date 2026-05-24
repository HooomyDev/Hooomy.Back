using FluentValidation;

namespace Hooome.Application.CQRS.Inquiries.Commands.CreateInquire;

public class CreateInquiryCommandValidator
    : AbstractValidator<CreateInquiryCommand>
{
    public CreateInquiryCommandValidator()
    {
        RuleFor(i => i.Message).NotEmpty().MaximumLength(2000);
        RuleFor(i => i.UserEmail).NotEmpty().MaximumLength(255);
    }
}