using FluentValidation;
using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.SubmitVote;

public class SubmitVoteCommandHandler(IPollRepository pollRepo, 
    IPollVoteRepository pollVoteRepo, IPollOptionRepository pollOptionRepo)
    : IRequestHandler<SubmitVoteCommand>
{
    public async Task Handle(SubmitVoteCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepo.GetById(request.PollId, cancellationToken)
            ?? throw new NotFoundException(nameof(Poll), request.PollId);

        var hasVoted = await pollVoteRepo
            .IsVoteExist(request.PollId, request.UserId, cancellationToken);

        if (hasVoted)
            throw new ValidationException("You already voted in this poll");

        switch (request.Vote.Type)
        {
            case PollType.One:
                {
                    var option = await pollOptionRepo.GetById(request.Vote.OptionId, cancellationToken)
                        ?? throw new NotFoundException(nameof(PollOption), request.Vote.OptionId);

                    var vote = new PollVote
                    {
                        Id = Guid.NewGuid(),
                        PollId = poll.Id,
                        OptionId = option.Id,
                        UserId = request.UserId,
                        CreatedAt = DateTime.UtcNow
                    };

                    await pollVoteRepo.Create(vote, cancellationToken);
                }
                break;

            case PollType.Several:
                {
                    if (request.Vote.OptionIds == null || request.Vote.OptionIds.Count == 0)
                        throw new ValidationException("Options is null");

                    foreach (var optionId in request.Vote.OptionIds)
                    {
                        var option = await pollOptionRepo.GetById(request.Vote.OptionId, cancellationToken)
                            ?? throw new NotFoundException(nameof(PollOption), request.Vote.OptionId);

                        var vote = new PollVote
                        {
                            Id = Guid.NewGuid(),
                            PollId = poll.Id,
                            OptionId = option.Id,
                            UserId = request.UserId,
                            CreatedAt = DateTime.UtcNow
                        };

                        await pollVoteRepo.Create(vote, cancellationToken);
                    }
                }
                break;

            default:
                throw new ValidationException("Unsupported poll type");
        }

        await pollRepo.SaveChanges(cancellationToken);
    }
}
