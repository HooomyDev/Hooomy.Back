using AutoMapper;
using Hooome.Application.Interfaces;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.RequestComments.Queries.GetRequestComments;

public class GetRequestCommentsQueryHandler(IRequestCommentRepository requestCommentRepo,
    IRequestCommentImageRepository requestCommentImageRepo,
    IMinioService minioService,
    IMapper mapper)
    : IRequestHandler<GetRequestCommentsQuery, RequestCommentsVm>
{
    public async Task<RequestCommentsVm> Handle(GetRequestCommentsQuery request, CancellationToken cancellationToken)
    {
        var (comments, totalCount) = await requestCommentRepo.GetByRequestId(
            text: request.Text,
            status: request.Status,
            requestId: request.RequestId,
            page: request.Page,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var commentDtos = mapper.Map<List<RequestCommentLookupDto>>(comments);

        foreach (var comment in commentDtos)
        {
            var images = await requestCommentImageRepo.GetByCommentId(comment.Id, cancellationToken);

            var photoUrls = new List<string>();
            foreach (var image in images)
            {
                var url = await minioService.GetUrl(ImageType.RequestComment, image.FileName);
                photoUrls.Add(url);
            }

            comment.PhotoUrls = photoUrls;
        }

        return new RequestCommentsVm()
        {
            RequestComments = commentDtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}