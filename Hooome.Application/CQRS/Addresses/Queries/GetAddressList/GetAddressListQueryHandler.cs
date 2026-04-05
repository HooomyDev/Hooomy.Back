using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressList;

public class GetAddressListQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetAddressListQuery, AddressListVm>
{
    public async Task<AddressListVm> Handle(GetAddressListQuery request, CancellationToken cancellationToken)
    {
        var addresses = await dbContext.Addresses
                .Where(s => s.Street.Contains(request.Query, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(s => s.Street)
                .Take(15)
                .ProjectTo<AddressListLookupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        return new AddressListVm { Addresses = addresses };
    }
}
