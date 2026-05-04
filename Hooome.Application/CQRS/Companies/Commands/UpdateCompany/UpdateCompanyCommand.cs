using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string WorkingHours { get; set; } = null!;
    public Guid AddressId { get; set; }
}
