using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.FavoriteAddresses.Commands.DeleteFavoriteAddress;

public class DeleteFavoriteAddressCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<DeleteFavoriteAddressCommand>
{
    public async Task Handle(DeleteFavoriteAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.FavoriteAddresses
          .FindAsync([request.Id], cancellationToken);

        if (entity == null || entity.UserID != request.UserId)
        {
            throw new NotFoundException(nameof(Request), request.Id);
        }

        dbContext.FavoriteAddresses.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
