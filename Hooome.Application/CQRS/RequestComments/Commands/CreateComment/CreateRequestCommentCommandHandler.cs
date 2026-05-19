using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Commands.CreateComment;

public class CreateRequestCommentCommandHandler(IRequestRepository requestRepo,
    IRequestCommentRepository requestCommentsRepo,
    ICompanyRepository companyRepo)
    : IRequestHandler<CreateRequestCommentCommand, Guid>
{
    public async Task<Guid> Handle(CreateRequestCommentCommand request, CancellationToken cancellationToken)
    {
        var requestExists = await requestRepo.IsExist(request.RequestId, cancellationToken);

        if (!requestExists)
            throw new NotFoundException(nameof(Request), request.RequestId);

        var companyExists = await companyRepo.IsExist(request.CompanyId, cancellationToken);
        
        if(!companyExists)
            throw new NotFoundException(nameof(Request), request.RequestId);

        var newRequestComment = new RequestComment()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            RequestId = request.RequestId,
            CompanyId = request.CompanyId,
            Text = request.Text,
            Status = Domain.Enums.RequestCommentStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        };

        await requestCommentsRepo.Create(newRequestComment, cancellationToken);

        return newRequestComment.Id;
    }
}
