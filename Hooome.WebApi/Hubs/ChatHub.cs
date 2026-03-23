using Hooome.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Hooome.WebApi.Hubs;

public interface IChatClient
{
    public Task ReceiveMessage(string userName, string message);
}

public class ChatHub(IDistributedCache cache) : Hub<IChatClient>
{
    public async Task JoinChat(UserConnection connection)
    {
        var chatName = connection.ChatId.ToString();
        await Groups
            .AddToGroupAsync(Context.ConnectionId, chatName);

        var stringConnection = JsonSerializer.Serialize(connection);

        await cache.SetStringAsync(Context.ConnectionId, stringConnection);

        await Clients
            .Group(chatName)
            .ReceiveMessage("system", $"{connection.UserName} присоединился к чату");
    }

    public async Task SendAsync(string message)
    {
        var stringConnection = await cache.GetAsync(Context.ConnectionId);

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if(connection is not null)
        {
            var chatName = connection.ChatId.ToString();

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

            await Clients
                .Group(chatName)
                .ReceiveMessage("system", $"{connection.UserName} вышел из чата");
        }
    }
}

public record UserConnection(string UserName, Guid ChatId);
