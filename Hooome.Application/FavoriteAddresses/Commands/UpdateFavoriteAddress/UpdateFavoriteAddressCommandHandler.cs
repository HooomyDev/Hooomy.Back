using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.FavoriteAddresses.Commands.UpdateFavoriteAddress;

public class UpdateFavoriteAddressCommandHandler(IHooomeDbContext dbContext) 
    : IRequestHandler<UpdateFavoriteAddressCommand>
{
    public async Task Handle(UpdateFavoriteAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.FavoriteAddresses
            .FindAsync([request.Id], cancellationToken);

        if (entity == null || entity.UserID != request.UserId)
        {
            throw new NotFoundException(nameof(FavoriteAddress), request.Id);
        }

        entity.Street = request.Street;
        entity.House = request.House;
        entity.Pseudonym = request.Pseudonym;
        entity.UpdatedAt = DateTime.Now;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
