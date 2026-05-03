using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.CreateComment;

public class CreateRequestCommentCommandHandler(IRequestRepository requestRepo,
    IRepository<RequestComment> requestCommentsRepo)
    : IRequestHandler<CreateRequestCommentCommand, Guid>
{
    public async Task<Guid> Handle(CreateRequestCommentCommand request, CancellationToken cancellationToken)
    {
        var requestExist = await requestRepo.IsExist(request.RequestId, cancellationToken);

        if (!requestExist)
            throw new NotFoundException(nameof(Request), request.RequestId);

        var newRequestComment = new RequestComment()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            RequestId = request.RequestId,
            SenderName = request.SenderName,
            Text = request.Text,
            PhotoUrl = "...",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        };

        await requestCommentsRepo.Create(newRequestComment, cancellationToken);

        return newRequestComment.Id;
    }
}
