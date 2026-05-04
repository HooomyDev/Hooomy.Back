using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.RemoveAddress;

public class RemoveAddressCommand : IRequest
{
    public Guid CompanyId { get; set; }
    public Guid AddressId { get; set; }
}
