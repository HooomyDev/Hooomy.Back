using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.Requests.Commands.CreateRequest;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class CreateRequestDto : IMapWith<CreateRequestCommand>
{
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public RequestCategory Category { get; set; } = RequestCategory.Other;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateRequestDto, CreateRequestCommand>();
}
