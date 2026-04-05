using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.UpdateRequest;

public class UpdateRequestCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid AddressId { get; set; }
    public RequestCategory Category { get; set; }
    public RequestStatus Status { get; set; }
}
