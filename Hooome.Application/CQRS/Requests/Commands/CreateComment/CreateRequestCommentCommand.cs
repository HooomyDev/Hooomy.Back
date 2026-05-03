using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.CreateComment;

public class CreateRequestCommentCommand : IRequest<Guid>
{
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public string SenderName { get; set; } = null!;
    public string Text { get; set; } = null!;
}
