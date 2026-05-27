using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Inquiries.Commands.CreateInquire;

namespace Hooome.WebApi.Models;

public class CreateInquiryDto : IMapWith<CreateInquiryCommand>
{
    public string Message { get; set; } = null!;
    public string UserEmail { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateInquiryDto, CreateInquiryCommand>();
}