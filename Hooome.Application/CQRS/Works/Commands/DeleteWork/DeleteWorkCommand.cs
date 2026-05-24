using MediatR;

namespace Hooome.Application.CQRS.Works.Commands.DeleteWork;

public class DeleteWorkCommand : IRequest
{
    public Guid WorkId { get; set; }
}
