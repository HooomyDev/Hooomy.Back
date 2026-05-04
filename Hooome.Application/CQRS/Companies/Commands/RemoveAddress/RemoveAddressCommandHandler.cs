using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.RemoveAddress;

public class RemoveAddressCommandHandler(ICompanyRepository companyRepo, IAddressRepository addressRepo)
    : IRequestHandler<RemoveAddressCommand>
{
    public async Task Handle(RemoveAddressCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepo.GetById(request.CompanyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), request.CompanyId);

        var address = await addressRepo.GetById(request.AddressId, cancellationToken)
            ?? throw new NotFoundException(nameof(Address), request.AddressId);

        await addressRepo.RemoveAddress(company.Id, address.Id, cancellationToken);
    }
}
