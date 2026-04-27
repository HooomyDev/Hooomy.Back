using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommand : IRequest
{
    public Guid CompanyId { get; set; }
}
