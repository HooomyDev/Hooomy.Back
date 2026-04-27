using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.AddAddress;

public class AddAddressCommandHandler(ICompanyRepository companyRepo, IAddressRepository addressRepo)
    : IRequestHandler<AddAddressCommand>
{
    public async Task Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepo.GetById(request.CompanyId, cancellationToken) 
            ?? throw new NotFoundException(nameof(Company), request.CompanyId);

        var address = await addressRepo.GetById(request.AddressId, cancellationToken)
            ?? throw new NotFoundException(nameof(Address), request.AddressId);

        await addressRepo.AddAddress(company.Id, address.Id, cancellationToken);
    }
}