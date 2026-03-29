using AutoMapper;
using Hooome.Application.Common.Mappings;
using Hooome.Application.CQRS.Chats.Commands.CreateChat;

namespace Hooome.WebApi.Models;

public class CreateChatDto : IMapWith<CreateChatCommand>
{
    public Guid CompanyId { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateChatDto, CreateChatCommand>();
}