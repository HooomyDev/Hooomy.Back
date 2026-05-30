using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(ICompanyRepository companyRepo, IAddressRepository addressRepo)
    : IRequestHandler<CreateCompanyCommand, Guid>
{
    public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var newCompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Phone = request.Phone ?? string.Empty,
            Email = request.Email ?? string.Empty,
            WorkingHours = request.WorkingHours ?? string.Empty,
            AddressId = request.AddressId,
            CreatedAt = DateTime.UtcNow,
        };

        if (request.AddressId is not null)
        {
            var address = await addressRepo.GetById(request.AddressId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Address), request.AddressId.Value);

            address.RegisteredCompanyId = newCompany.Id;
        }

        await companyRepo.Create(newCompany, cancellationToken);

        return newCompany.Id;
    }
}
