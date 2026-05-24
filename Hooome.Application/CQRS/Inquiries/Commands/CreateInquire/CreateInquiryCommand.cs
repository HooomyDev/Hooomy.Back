using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Inquiries.Commands.CreateInquire;

public class CreateInquiryCommand : IRequest<Guid>, IMapWith<Inquiry>
{
    public string Message { get; set; } = null!;
    public string UserEmail { get; set; } = null!;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateInquiryCommand, Inquiry>();
}
