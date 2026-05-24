using MediatR;

namespace Hooome.Application.CQRS.Inquiries.Queries.GetInquiryList;

public class GetInquiryListQuery : IRequest<InquiryListVm>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public DateTime? Date { get; set; }
}
