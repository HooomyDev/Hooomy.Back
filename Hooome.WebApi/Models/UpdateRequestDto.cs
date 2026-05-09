using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Requests.Commands.UpdateRequest;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class UpdateRequestDto : IMapWith<UpdateRequestCommand>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; } 
    public RequestCategory? Category { get; set; }
    public RequestStatus? Status { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateRequestDto, UpdateRequestCommand>();
    }
}
