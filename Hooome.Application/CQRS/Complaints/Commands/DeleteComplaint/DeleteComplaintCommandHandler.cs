using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Complaints.Commands.DeleteComplaint;

public class DeleteComplaintCommandHandler(IComplaintRepository complaintRepo)
    : IRequestHandler<DeleteComplaintCommand>
{
    public async Task Handle(DeleteComplaintCommand request, CancellationToken cancellationToken)
    {
        var entity = await complaintRepo.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Complaints), request.Id);

        await complaintRepo.Delete(entity, cancellationToken);
    }
}
