using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.CreateFavoriteAddress;

public class CreateFavoriteAddressCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<CreateFavoriteAddressCommand, Guid>
{
    public async Task<Guid> Handle(CreateFavoriteAddressCommand request, 
        CancellationToken cancellationToken)
    {
        var newFavoriteAddress = new FavoriteAddress
        {
            Id = Guid.NewGuid(),
            UserID = request.UserId,
            Street = request.Street,
            House = request.House,
            Pseudonym = request.Pseudonym,
            CreatedAt = DateTime.Now,
            UpdatedAt = null,
        };

        await dbContext.FavoriteAddresses.AddAsync(newFavoriteAddress, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return newFavoriteAddress.Id;
    }
}
