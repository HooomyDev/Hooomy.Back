using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;

namespace Hooome.Application.CQRS.Inquiries.Queries.GetInquiryList;

public class InquiryListLookupDto : IMapWith<Inquiry>
{
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Inquiry, InquiryListLookupDto>();
}
