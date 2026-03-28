using FluentValidation;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Polls.Commands.SubmitVote;

public class SubmitVoteCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<SubmitVoteCommand>
{
    public async Task Handle(SubmitVoteCommand request, CancellationToken cancellationToken)
    {
        var poll = await dbContext.Polls
            .Include(p => p.Options)
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == request.PollId, cancellationToken)
            ?? throw new NotFoundException(nameof(Poll), request.PollId);

        var hasVoted = await dbContext.PollVotes
            .AnyAsync(v => v.PollId == request.PollId && v.UserId == request.UserId, cancellationToken);

        if (hasVoted)
            throw new ValidationException("Вы уже проголосовали в этом опросе");

        switch (request.Vote.Type)
        {
            case PollType.One:
                {
                    if (!request.Vote.OptionId.HasValue)
                        throw new ValidationException("Не выбран вариант ответа");

                    var option = poll.Options.FirstOrDefault(o => o.Id == request.Vote.OptionId.Value)
                        ?? throw new NotFoundException(nameof(PollOption), request.Vote.OptionId.Value);

                    var vote = new PollVote
                    {
                        Id = Guid.NewGuid(),
                        PollId = poll.Id,
                        OptionId = option.Id,
                        UserId = request.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await dbContext.PollVotes.AddAsync(vote, cancellationToken);
                }
                break;

            case PollType.Several:
                {
                    if (request.Vote.OptionIds == null || request.Vote.OptionIds.Count == 0)
                        throw new ValidationException("Не выбрано ни одного варианта ответа");

                    foreach (var optionId in request.Vote.OptionIds)
                    {
                        var option = poll.Options.FirstOrDefault(o => o.Id == optionId)
                            ?? throw new NotFoundException(nameof(PollOption), optionId);

                        var vote = new PollVote
                        {
                            Id = Guid.NewGuid(),
                            PollId = poll.Id,
                            OptionId = option.Id,
                            UserId = request.UserId,
                            CreatedAt = DateTime.UtcNow
                        };

                        await dbContext.PollVotes.AddAsync(vote, cancellationToken);
                    }
                }
                break;

            default:
                throw new ValidationException("Неподдерживаемый тип опроса");
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
