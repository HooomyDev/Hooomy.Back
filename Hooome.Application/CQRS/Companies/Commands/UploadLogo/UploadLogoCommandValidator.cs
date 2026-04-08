using FluentValidation;

namespace Hooome.Application.CQRS.Companies.Commands.UploadLogo;

public class UploadLogoCommandValidator
    : AbstractValidator<UploadLogoCommand>
{
    public UploadLogoCommandValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(x => x.File).NotEmpty();
    }
}