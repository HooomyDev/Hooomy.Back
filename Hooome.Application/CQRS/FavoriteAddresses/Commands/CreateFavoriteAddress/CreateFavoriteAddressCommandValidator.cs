using FluentValidation;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.CreateFavoriteAddress;

public class CreateFavoriteAddressCommandValidator
    : AbstractValidator<CreateFavoriteAddressCommand>
{
    public CreateFavoriteAddressCommandValidator()
    {
        RuleFor(fa => fa.UserId).NotEmpty();
        RuleFor(fa => fa.Street).NotEmpty().MaximumLength(100);
        RuleFor(fa => fa.Pseudonym).NotEmpty().MaximumLength(100);
        RuleFor(fa => fa.House).NotEmpty().GreaterThan(0);
    }
}
