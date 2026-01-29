using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.Requests.Commands.CreateRequest;
using Hooome.Domain.Enums;

namespace Hooome.WebApi.Models;

public class CreateRequestDto : IMapWith<CreateRequestCommand>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public RequestCategory Category { get; set; } = RequestCategory.Other;
    public string PhotoUrl { get; set; } = string.Empty;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateRequestDto, CreateRequestCommand>();
}
