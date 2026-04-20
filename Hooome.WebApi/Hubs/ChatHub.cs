using AutoMapper;
using Hooome.Application.CQRS.Messages.Commands.CreateMessage;
using Hooome.Application.Interfaces;
using Hooome.Domain.Enums;
using Hooome.WebApi.Models;
using Hooome.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Hooome.WebApi.Hubs;

public interface IChatClient
{
    public Task ReceiveMessage(string userName, MessageDetailsVm message);
}

[Authorize]
public class ChatHub(IDistributedCache cache, IMapper mapper, IChatModerationService moderationService) : BaseHub<IChatClient>
{
    public async Task JoinChat(UserConnection connection)
    {
        var chatName = connection.ChatId.ToString();
        await Groups
            .AddToGroupAsync(Context.ConnectionId, chatName);

        var stringConnection = JsonSerializer.Serialize(connection);

        await cache.SetStringAsync(Context.ConnectionId, stringConnection);

        var message = new MessageDetailsVm()
        {
            Id = Guid.Empty,
            SenderName = "system",
            SenderType = SenderType.Unknown,
            MessageType = MessageType.System,
            Content = $"{connection.UserName} присоединился к чату",
            IsRead = false,
            ReadAt = null,
            CreatedAt = DateTime.UtcNow,
        };

        await Clients
            .Group(chatName)
            .ReceiveMessage("system", message);
    }

    public async Task SendAsync(CreateMessageDto messageDto)
    {
        var stringConnection = await cache.GetAsync(Context.ConnectionId);

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if(connection is not null)
        {
            messageDto.Content = moderationService.Filter(messageDto.Content);

            var chatName = connection.ChatId.ToString();

            var command = mapper.Map<CreateMessageCommand>(messageDto);
            command.SenderId = UserId;

            var message = await Mediator.Send(command);

            await Clients
                .Group(chatName)
                .ReceiveMessage(connection.UserName, message);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringConnection = await cache.GetAsync(Context.ConnectionId);

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if(connection is not null)
        {
            var chatName = connection.ChatId.ToString();

            await cache.RemoveAsync(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);

            var message = new MessageDetailsVm()
            {
                Id = Guid.Empty,
                SenderName = "system",
                SenderType = SenderType.Unknown,
                MessageType = MessageType.System,
                Content = $"{connection.UserName} вышел из чата",
                IsRead = false,
                ReadAt = null,
                CreatedAt = DateTime.UtcNow,
            };

            await Clients
                .Group(chatName)
                .ReceiveMessage("system", message);
        }
    }
}
