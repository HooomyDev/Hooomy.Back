using FluentValidation;

namespace Hooome.Application.CQRS.Requests.Commands.UploadImages;

public class UploadImagesCommandValidator
    : AbstractValidator<UploadImagesCommand>
{
    public UploadImagesCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(x => x.RequestId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(x => x.Files).NotEmpty();
    }
}