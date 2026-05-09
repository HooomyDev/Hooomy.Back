using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.CQRS.Polls.Commands.CreatePoll;

public class CreatePollCommandHandler(IPollRepository pollRepo, 
    IPollOptionRepository pollOptionRepo)
    : IRequestHandler<CreatePollCommand, Guid>
{
    public async Task<Guid> Handle(CreatePollCommand request, CancellationToken cancellationToken)
    {
        var newPoll = new Poll()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CreatedBy = request.CreatedBy,
            CompanyId = request.CompanyId,
            Status = PollStatus.Active,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await pollRepo.Create(newPoll, cancellationToken);

        foreach (var optionDto in request.Options)
        {
            var option = new PollOption
            {
                Id = Guid.NewGuid(),
                PollId = newPoll.Id,
                Content = optionDto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            await pollOptionRepo.Create(option, cancellationToken);
        }

        return newPoll.Id;
    }
}
