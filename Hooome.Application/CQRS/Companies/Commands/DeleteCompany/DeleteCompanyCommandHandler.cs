using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommandHandler(ICompanyRepository companyRepo)
    : IRequestHandler<DeleteCompanyCommand>
{
    public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepo.GetById(request.CompanyId, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), request.CompanyId);

        await companyRepo.Delete(company, cancellationToken);
    }
}