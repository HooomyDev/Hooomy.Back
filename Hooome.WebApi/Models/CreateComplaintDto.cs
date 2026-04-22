using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Complaints.Commands.CreateComplaint;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class CreateComplaintDto : IMapWith<CreateComplaintCommand>
{
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintType Type { get; set; } = ComplaintType.Unknown;
    public Guid? RequestId { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateComplaintDto, CreateComplaintCommand>();
}
