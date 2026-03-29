using MediatR;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.DeleteFavoriteAddress;

public class DeleteFavoriteAddressCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
