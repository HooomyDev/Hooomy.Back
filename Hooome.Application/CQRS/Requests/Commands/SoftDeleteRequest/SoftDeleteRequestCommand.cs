using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.SoftDeleteRequest;

public class SoftDeleteRequestCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
