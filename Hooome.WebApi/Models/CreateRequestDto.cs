using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Requests.Commands.CreateRequest;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class CreateRequestDto : IMapWith<CreateRequestCommand>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid AddressId { get; set; }
    public RequestCategory Category { get; set; } = RequestCategory.Other;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateRequestDto, CreateRequestCommand>();
}
