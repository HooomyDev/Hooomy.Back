using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.Requests.Commands.UpdateRequest;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class UpdateRequestDto : IMapWith<UpdateRequestCommand>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public RequestCategory Category { get; set; }
    public RequestStatus Status { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;

    public void Mapping(Profile profile)
        => profile.CreateMap<UpdateRequestDto, UpdateRequestCommand>();
}
