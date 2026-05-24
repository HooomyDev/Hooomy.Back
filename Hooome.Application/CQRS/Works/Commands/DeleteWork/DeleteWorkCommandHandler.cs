using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Works.Commands.DeleteWork;

public class DeleteWorkCommandHandler(IWorkRepository workRepo)
    : IRequestHandler<DeleteWorkCommand>
{
    public async Task Handle(DeleteWorkCommand request, CancellationToken cancellationToken)
    {
        var work = await workRepo.GetById(request.WorkId, cancellationToken)
            ?? throw new NotFoundException(nameof(Work), request.WorkId);

        await workRepo.Delete(work, cancellationToken);
    }
}
