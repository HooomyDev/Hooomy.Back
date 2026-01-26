using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.Requests.Commands.CreateRequest;

public class CreateRequestCommandHandler(IHooomeDbContext dbContext) 
    : IRequestHandler<CreateRequestCommand, Guid>
{
    private readonly IHooomeDbContext _dbContext = dbContext;

    public async Task<Guid> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {
        var newRequest = new Request
        {
            Id = Guid.NewGuid(),
            UserID = request.UserId,
            Description = request.Description,
            Address = request.Address,
            Category = request.Category,
            Status = Domain.Enums.RequestStatus.Created,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        await _dbContext.Requests.AddAsync(newRequest, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return newRequest.Id;
    }
}
