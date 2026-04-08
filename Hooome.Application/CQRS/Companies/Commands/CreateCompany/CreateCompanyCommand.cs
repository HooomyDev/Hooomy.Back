using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.CreateCompany;

public class CreateCompanyCommand : IRequest<Guid>
{
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? WorkingHours { get; set; }
    public Guid? AddressId { get; set; }
}
