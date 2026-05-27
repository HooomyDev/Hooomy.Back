namespace Hooome.Application.CQRS.Inquiries.Queries.GetInquiryList;

public class InquiryListVm
{
    public IList<InquiryListLookupDto> Inquiries { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
