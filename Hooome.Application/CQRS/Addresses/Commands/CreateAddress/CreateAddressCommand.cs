using MediatR;
using Serilog;
using System.IO;
using System.Net;

namespace Hooome.Application.CQRS.Addresses.Commands.CreateAddress;

public class CreateAddressCommand : IRequest<Guid>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; } = null!;
}
