using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.FavoriteAddresses.Queries.GetFavoriteAddressList;

public class GetFavoriteAddressListQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetFavoriteAddressListQuery, FavoriteAddressListVm>
{
    public async Task<FavoriteAddressListVm> Handle(GetFavoriteAddressListQuery request, CancellationToken cancellationToken)
    {
        var favoriteAddresses = await dbContext.FavoriteAddresses
            .Where(fa => fa.UserId == request.UserId)
            .Include(fa => fa.Address)
            .Select(fa => new FavoriteAddressListDto
            {
                Id = fa.Id,
                Street = fa.Address.Street,
                House = fa.Address.HouseNumber,
                Pseudonym = fa.Pseudonym
            })
            .ToListAsync(cancellationToken);

        return new FavoriteAddressListVm { FavoriteAddresses = favoriteAddresses };
    }
}
