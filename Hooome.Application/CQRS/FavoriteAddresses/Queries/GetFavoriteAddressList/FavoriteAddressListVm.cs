namespace Hooome.Application.CQRS.FavoriteAddresses.Queries.GetFavoriteAddressList;

public class FavoriteAddressListVm
{
    public IList<FavoriteAddressListDto> FavoriteAddresses { get; set; } = [];
}
