using AutoMapper;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Works.Commands.UpdateWork;

public class UpdateWorkCommandHandler(IWorkRepository workRepo, IMapper mapper)
    : IRequestHandler<UpdateWorkCommand>
{
    public async Task Handle(UpdateWorkCommand request, CancellationToken cancellationToken)
    {
        var work = await workRepo.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Work), request.Id);

        mapper.Map(request, work);
        work.UpdatedAt = DateTime.UtcNow;

        await workRepo.SaveChanges(cancellationToken);
    }
}