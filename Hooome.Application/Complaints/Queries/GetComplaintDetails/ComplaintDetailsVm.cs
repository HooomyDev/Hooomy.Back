using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Complaints.Queries.GetComplaintDetails;

public class ComplaintDetailsVm : IMapWith<Complaint>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintType Type { get; set; } = ComplaintType.Unknown;
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Unknown;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public Guid? RequestId { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<Complaint, ComplaintDetailsVm>();
}
