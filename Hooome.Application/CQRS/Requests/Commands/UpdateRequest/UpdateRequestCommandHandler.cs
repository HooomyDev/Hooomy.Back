using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandHandler(IRequestRepository requestRepo) 
    : IRequestHandler<UpdateRequestCommand>
{
    public async Task Handle(UpdateRequestCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = await requestRepo.GetById(request.Id, cancellationToken);

        if (entity == null || entity.UserID != request.UserId)
        {
            throw new NotFoundException(nameof(Request), request.Id);
        }

        entity.Description = request.Description;
        entity.Category = request.Category;
        entity.AddressId = request.AddressId;
        entity.Status = request.Status;
        entity.UpdatedAt = DateTime.UtcNow;

        await requestRepo.Update(entity, cancellationToken);
    }
}
