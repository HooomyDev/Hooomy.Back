using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandHandler(ICompanyRepository companyRepo, IAddressRepository addressRepo, IMapper mapper)
    : IRequestHandler<UpdateCompanyCommand>
{
    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepo.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), request.Id);

        var oldAddressId = company.AddressId;
        if (oldAddressId != request.AddressId)
        {
            if (oldAddressId is not null && oldAddressId != Guid.Empty)
            {
                var oldAddress = await addressRepo.GetById(oldAddressId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(Address), oldAddressId.Value);

                oldAddress.RegisteredCompanyId = null;
            }

            if (request.AddressId != Guid.Empty)
            {
                var newAddress = await addressRepo.GetById(request.AddressId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Address), request.AddressId);

                newAddress.RegisteredCompanyId = company.Id;
            }
        }

        mapper.Map(request, company);

        company.UpdatedAt = DateTime.UtcNow;

        await companyRepo.SaveChanges(cancellationToken);
    }
}