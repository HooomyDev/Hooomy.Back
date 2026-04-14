using Hooome.Application.Common.Exceptions;
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
        var address = await dbContext.Addresses
            .FindAsync([request.AddressId], cancellationToken)
            ?? throw new NotFoundException(nameof(Address), request.AddressId);

        var newFavoriteAddress = new FavoriteAddress
        {
            Id = Guid.NewGuid(),
            UserID = request.UserId,
            AddressId = request.AddressId,
            Pseudonym = request.Pseudonym,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        };

        await dbContext.FavoriteAddresses.AddAsync(newFavoriteAddress, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return newFavoriteAddress.Id;
    }
}
