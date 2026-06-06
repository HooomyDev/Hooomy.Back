using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.FavoriteAddresses.Commands.UpdateFavoriteAddress;

public class UpdateFavoriteAddressCommandHandler(IHooomeDbContext dbContext) 
    : IRequestHandler<UpdateFavoriteAddressCommand>
{
    public async Task Handle(UpdateFavoriteAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.FavoriteAddresses
            .FindAsync([request.Id], cancellationToken);

        if (entity == null || entity.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(FavoriteAddress), request.Id);
        }

        entity.Pseudonym = request.Pseudonym;
        entity.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
