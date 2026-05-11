using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.UpdateRequest;

public class UpdateRequestCommandHandler(IRequestRepository requestRepo, IMapper mapper) 
    : IRequestHandler<UpdateRequestCommand>
{
    public async Task Handle(UpdateRequestCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = await requestRepo.GetById(request.Id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Request), request.Id);

        mapper.Map(request, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        
        await requestRepo.SaveChanges(cancellationToken);
    }
}
