using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Commands.UpdateComment;

public class UpdateRequestCommentCommandHandler(IRequestCommentRepository requestCommentRepo,
    IMapper mapper)
    : IRequestHandler<UpdateRequestCommentCommand>
{
    public async Task Handle(UpdateRequestCommentCommand request, CancellationToken cancellationToken)
    {
        var requestComment = await requestCommentRepo.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(RequestComment), request.Id);

        mapper.Map(request, requestComment);
        requestComment.UpdatedAt = DateTime.UtcNow;

        await requestCommentRepo.SaveChanges(cancellationToken);
    }
}