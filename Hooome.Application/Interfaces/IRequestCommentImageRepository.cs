using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IRequestCommentImageRepository : IRepository<RequestCommentImage>
{
    Task<ICollection<RequestCommentImage>> GetByCommentId(Guid commentId, CancellationToken cancellationToken = default);
}
