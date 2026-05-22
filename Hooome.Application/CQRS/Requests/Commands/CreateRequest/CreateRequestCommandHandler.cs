using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandler(
    IRepository<Address> addressRepo, 
    IRequestRepository requestRepo)
    : IRequestHandler<CreateRequestCommand, Guid>
{
    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var address = await addressRepo.GetById(request.AddressId, cancellationToken)
            ?? throw new NotFoundException(nameof(Address), request.AddressId);

        var newRequest = new Request
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            AddressId = request.AddressId,
            Category = request.Category,
            Status = Domain.Enums.RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await requestRepo.Create(newRequest, cancellationToken);

        return newRequest.Id;
    }
}
