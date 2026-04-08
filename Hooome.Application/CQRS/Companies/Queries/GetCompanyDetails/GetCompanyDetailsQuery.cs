using MediatR;

namespace Hooome.Application.CQRS.Companies.Queries.GetCompanyDetails;

public class GetCompanyDetailsQuery : IRequest<CompanyDetailsVm>
{
    public Guid CompanyId { get; set; }
}
