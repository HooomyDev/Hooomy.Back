using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressList;

public class GetAddressListQueryHandler(IAddressRepository addressRepo, IMapper mapper)
    : IRequestHandler<GetAddressListQuery, AddressListVm>
{
    public async Task<AddressListVm> Handle(GetAddressListQuery request, CancellationToken cancellationToken)
    {
        var addresses = await addressRepo.GetByQuery(request.Query, cancellationToken);

        var addressDtos = mapper.Map<List<AddressListLookupDto>>(addresses);

        return new AddressListVm { Addresses = addressDtos };
    }
}
