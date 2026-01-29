using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.FavoriteAddresses.Queries.GetFavoriteAddressList;

public class GetFavoriteAddressListQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetFavoriteAddressListQuery, FavoriteAddressListVm>
{
    public async Task<FavoriteAddressListVm> Handle(GetFavoriteAddressListQuery request, CancellationToken cancellationToken)
    {
        var favoriteAddresses = await dbContext.FavoriteAddresses
            .Where(fa => fa.UserID == request.UserId)
            .ProjectTo<FavoriteAddressListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new FavoriteAddressListVm { FavoriteAddresses = favoriteAddresses };
    }
}
