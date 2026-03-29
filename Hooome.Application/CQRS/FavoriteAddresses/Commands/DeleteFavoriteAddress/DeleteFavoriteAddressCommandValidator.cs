using FluentValidation;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.DeleteFavoriteAddress;

public class DeleteFavoriteAddressCommandValidator
    : AbstractValidator<DeleteFavoriteAddressCommand>
{
    public DeleteFavoriteAddressCommandValidator()
    {
        RuleFor(fa => fa.UserId).NotEqual(Guid.Empty);
        RuleFor(fa => fa.Id).NotEqual(Guid.Empty);
    }
}