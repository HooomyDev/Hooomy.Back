using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.DeleteRequest;

public class DeleteRequestCommandHandler(IHooomeDbContext dbContext) 
    : IRequestHandler<DeleteRequestCommand>
{
    public async Task Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Requests
            .FindAsync([request.Id], cancellationToken);

        if(entity is null || entity.UserID != request.UserId)
        {
            throw new NotFoundException(nameof(Request), request.Id);
        }

        dbContext.Requests.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
