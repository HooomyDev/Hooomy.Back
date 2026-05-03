using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandHandler(ICompanyRepository companyRepo, IMapper mapper)
    : IRequestHandler<UpdateCompanyCommand>
{
    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await companyRepo.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Company), request.Id);

        mapper.Map(request, company);

        company.UpdatedAt = DateTime.UtcNow;

        await companyRepo.SaveChanges(cancellationToken);
    }
}