using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Queries.GetRequestCommentCount;

public class GetRequestCommentCountQueryHandler(IRequestCommentRepository requestCommentRepo)
    : IRequestHandler<GetRequestCommentCountQuery, int>
{
    public async Task<int> Handle(GetRequestCommentCountQuery request, CancellationToken cancellationToken)
    {
        var requestCommentCount = await requestCommentRepo.CountWithFilter(
            requestId: request.RequestId,
            filter: request.Filter,
            cancellationToken: cancellationToken);

        return requestCommentCount;
    }
}