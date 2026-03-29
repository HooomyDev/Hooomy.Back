using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.CQRS.Complaints.Queries.GetComplaintList;

public class ComplaintListLookupDto : IMapWith<Complaint>
{
    public Guid Id { get; set; }
    public string ShortDescription { get; set; } = null!;
    public ComplaintType Type { get; set; } = ComplaintType.Unknown;
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Unknown;

    public void Mapping(Profile profile)
        => profile.CreateMap<Complaint, ComplaintListLookupDto>();
}
