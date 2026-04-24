using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(ICompanyRepository companyRepo)
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
            AddressId = request.AddressId ?? null,
            CreatedAt = DateTime.UtcNow,
        };
        
        await companyRepo.Create(newCompany, cancellationToken);

        return newCompany.Id;
    }
}
