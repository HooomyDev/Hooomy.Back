using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandHandler(IHooomeDbContext dbContext) 
    : IRequestHandler<UpdateRequestCommand>
{
    private readonly IHooomeDbContext _dbContext = dbContext;

    public async Task Handle(UpdateRequestCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Requests
            .FindAsync([request.Id], cancellationToken);

        if (entity == null || entity.UserID != request.UserId)
        {
            throw new NotFoundException(nameof(Request), request.Id);
        }

        entity.Description = request.Description;
        entity.Category = request.Category;
        entity.Address = request.Address;
        entity.Status = request.Status;
        entity.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
