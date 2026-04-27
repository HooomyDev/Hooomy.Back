using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandHandler(ICompanyRepository companyRepo)
    : IRequestHandler<UpdateCompanyCommand>
{
    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepo.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), request.Id);

        company.Name = request.Name;
        company.Phone = request.Phone;
        company.Email = request.Email;
        company.WorkingHours = request.WorkingHours;
        company.AddressId = request.AddressId;
        company.UpdatedAt = DateTime.UtcNow;

        await companyRepo.Update(company, cancellationToken);
    }
}