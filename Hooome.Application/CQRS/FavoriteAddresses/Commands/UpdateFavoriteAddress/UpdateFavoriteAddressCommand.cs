using MediatR;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.UpdateFavoriteAddress;

public class UpdateFavoriteAddressCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Street { get; set; } = null!;
    public int House { get; set; }
    public string Pseudonym { get; set; } = null!;
}
