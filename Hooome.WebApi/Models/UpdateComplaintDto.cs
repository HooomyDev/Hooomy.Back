using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Complaints.Commands.UpdateComplaint;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class UpdateComplaintDto : IMapWith<UpdateComplaintCommand>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ShortDescription { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Unknown;

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateComplaintDto, UpdateComplaintCommand>();
}