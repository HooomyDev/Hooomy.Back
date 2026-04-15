using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandler(IHooomeDbContext dbContext) 
    : IRequestHandler<CreateRequestCommand, Guid>
{
    private readonly IHooomeDbContext _dbContext = dbContext;

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var address = await _dbContext.Addresses
            .FindAsync([request.AddressId], cancellationToken)
            ?? throw new NotFoundException(nameof(Address), request.AddressId);

        var newRequest = new Request
        {
            Id = Guid.NewGuid(),
            UserID = request.UserId,
            Title = request.Title,
            Description = request.Description,
            AddressId = request.AddressId,
            Category = request.Category,
            Status = Domain.Enums.RequestStatus.Created,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _dbContext.Requests.AddAsync(newRequest, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return newRequest.Id;
    }
}
