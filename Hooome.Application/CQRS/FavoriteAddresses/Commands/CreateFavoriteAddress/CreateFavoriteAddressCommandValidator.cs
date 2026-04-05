using FluentValidation;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.CreateFavoriteAddress;

public class CreateFavoriteAddressCommandValidator
    : AbstractValidator<CreateFavoriteAddressCommand>
{
    public CreateFavoriteAddressCommandValidator()
    {
        RuleFor(fa => fa.UserId).NotEmpty();
        RuleFor(fa => fa.AddressId).NotEmpty();
        RuleFor(fa => fa.Pseudonym).NotEmpty().MaximumLength(100);
    }
}
