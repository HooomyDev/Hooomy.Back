using MediatR;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.CreateFavoriteAddress;

public class CreateFavoriteAddressCommand
    : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Street { get; set; } = null!;
    public int House { get; set; }
    public string Pseudonym { get; set; } = null!;
}
