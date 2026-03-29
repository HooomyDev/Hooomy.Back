using MediatR;

namespace Hooome.Application.Requests.Commands.DeleteRequest;

public class DeleteRequestCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}
