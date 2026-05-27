using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressDetails;

public class GetAddressDetailsQueryHandler(IAddressRepository addressRepo, IMapper mapper)
    : IRequestHandler<GetAddressDetailsQuery, AddressDetailsVm>
{
    public async Task<AddressDetailsVm> Handle(GetAddressDetailsQuery request, CancellationToken cancellationToken)
    {
        var address = await addressRepo.GetById(request.AddressId, cancellationToken)
            ?? throw new NotFoundException(nameof(Address), request.AddressId);

        var addressVm = mapper.Map<AddressDetailsVm>(address);

        return addressVm;
    }
}
