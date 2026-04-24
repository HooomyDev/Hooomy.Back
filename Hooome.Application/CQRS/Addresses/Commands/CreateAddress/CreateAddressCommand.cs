using MediatR;

namespace Hooome.Application.CQRS.Addresses.Commands.CreateAddress;

public class CreateAddressCommand : IRequest<Guid>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; } = null!;
}
