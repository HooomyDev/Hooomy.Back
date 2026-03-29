using MediatR;

namespace Hooome.Application.CQRS.FavoriteAddresses.Queries.GetFavoriteAddressList;

public class GetFavoriteAddressListQuery : IRequest<FavoriteAddressListVm>
{
    public Guid UserId { get; set; }
}
