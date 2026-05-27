using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;

namespace Hooome.Application.CQRS.Inquiries.Queries.GetInquiryList;

public class GetInquiryListQueryHandler(IInquiryRepository inquiryRepo, IMapper mapper)
    : IRequestHandler<GetInquiryListQuery, InquiryListVm>
{
    public async Task<InquiryListVm> Handle(GetInquiryListQuery request, CancellationToken cancellationToken)
    {
        var (inquiries, totalCount) = await inquiryRepo.GetAllWithPagination(
            page: request.Page,
            pageSize: request.PageSize,
            date: request.Date,
            cancellationToken: cancellationToken);

        var inquiresDtos = mapper.Map<List<InquiryListLookupDto>>(inquiries);

        return new InquiryListVm
        {
            Inquiries = inquiresDtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}
