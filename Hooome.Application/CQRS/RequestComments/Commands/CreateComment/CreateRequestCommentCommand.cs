using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Commands.CreateComment;

public class CreateRequestCommentCommand : IRequest<Guid>
{
    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public string Text { get; set; } = null!;
}
