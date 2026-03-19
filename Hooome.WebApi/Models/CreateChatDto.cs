using AutoMapper;
using Hooome.Application.Chats.Commands.CreateChat;
using Hooome.Application.Common.Mappings;

namespace Hooome.WebApi.Models;

public class CreateChatDto : IMapWith<CreateChatCommand>
{
    public Guid CompanyId { get; set; }

    public void Mapping(Profile profile)
        => profile.CreateMap<CreateChatDto, CreateChatCommand>();
}