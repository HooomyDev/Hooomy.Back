using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.DeleteRequest;

public class DeleteRequestCommandHandler(IRepository<Request> requestRepo) 
    : IRequestHandler<DeleteRequestCommand>
{
    public async Task Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await requestRepo.GetById(request.Id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Request), request.Id);
        
        await requestRepo.Delete(entity, cancellationToken);
    }
}
