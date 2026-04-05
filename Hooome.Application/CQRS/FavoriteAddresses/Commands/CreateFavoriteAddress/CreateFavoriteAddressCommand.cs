using MediatR;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.CreateFavoriteAddress;

public class CreateFavoriteAddressCommand
    : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
    public string Pseudonym { get; set; } = null!;
}
