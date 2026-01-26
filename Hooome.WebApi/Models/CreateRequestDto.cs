using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.Requests.Commands.CreateRequest;
using Hooome.Application.Requests.Queries.GetRequestList;
using Hooome.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Hooome.WebApi.Models;

public class CreateRequestDto : IMapWith<CreateRequestCommand>
{
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public RequestStatus Status { get; set; } = RequestStatus.Unknown;
    public RequestCategory Category { get; set; } = RequestCategory.Other;

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateRequestDto, CreateRequestCommand>();
}
