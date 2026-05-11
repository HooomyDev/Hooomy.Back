using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Requests.Commands.UpdateRequest;

public class UpdateRequestCommand : IRequest, IMapWith<Request>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public RequestCategory Category { get; set; }
    public RequestStatus Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateRequestCommand, Request>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
