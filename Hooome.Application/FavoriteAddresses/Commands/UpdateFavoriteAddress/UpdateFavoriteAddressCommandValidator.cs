using FluentValidation;

namespace Hooome.Application.FavoriteAddresses.Commands.UpdateFavoriteAddress;

public class UpdateFavoriteAddressCommandValidator
    : AbstractValidator<UpdateFavoriteAddressCommand>
{
    public UpdateFavoriteAddressCommandValidator()
    {
        RuleFor(fa => fa.Id).NotEmpty();
        RuleFor(fa => fa.UserId).NotEmpty();
        RuleFor(fa => fa.Street).NotEmpty().MaximumLength(100);
        RuleFor(fa => fa.Pseudonym).NotEmpty().MaximumLength(100);
        RuleFor(fa => fa.House).NotEmpty().GreaterThan(0);
    }
}