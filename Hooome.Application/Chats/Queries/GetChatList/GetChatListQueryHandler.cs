using AutoMapper;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.Chats.Queries.GetChatList;

public class GetChatListQueryHandler(IHooomeDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetChatListQuery, ChatListVm>
{
    public async Task<ChatListVm> Handle(GetChatListQuery request, CancellationToken cancellationToken)
    {
        //get all chats for user
        var chatsQuery = dbContext.Chats
            .Where(x => x.ResidentId == request.UserId);

        //map to ChatListLookupDto with sub-queries for last message data
        var chats = await chatsQuery.Select(x => new ChatListLookupDto
        {
            Id = x.Id,
            CompanyName = x.Company.Name,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            LastMessageContent = dbContext.Messages
                .Where(m => m.ChatId == x.Id)
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => m.Content)
                .FirstOrDefault()
                    ?? "Сообщений пока нет, напишите первым!",
            LastMessageSentAt = dbContext.Messages
                    .Where(m => m.ChatId == x.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.CreatedAt)
                    .FirstOrDefault()
        })
            .OrderByDescending(x => x.LastMessageSentAt ?? x.UpdatedAt)
            .ToListAsync(cancellationToken);

        return new ChatListVm { Chats = chats };
    }
}
