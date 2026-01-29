using MediatR;

namespace Hooome.Application.FavoriteAddresses.Queries.GetFavoriteAddressList;

public class GetFavoriteAddressListQuery : IRequest<FavoriteAddressListVm>
{
    public Guid UserId { get; set; }
}
