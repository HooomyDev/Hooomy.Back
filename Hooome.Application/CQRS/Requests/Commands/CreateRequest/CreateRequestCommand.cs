using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.CreateRequest;

public class CreateRequestCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid AddressId { get; set; } 
    public RequestCategory Category { get; set; }
}
