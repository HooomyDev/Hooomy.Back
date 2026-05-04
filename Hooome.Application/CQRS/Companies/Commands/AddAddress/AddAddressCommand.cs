using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.AddAddress;

public class AddAddressCommand : IRequest
{
    public Guid CompanyId { get; set; }
    public Guid AddressId { get; set; }
}
