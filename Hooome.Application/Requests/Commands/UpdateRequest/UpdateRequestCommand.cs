using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.Requests.Commands.UpdateRequest;

public class UpdateRequestCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public RequestCategory Category { get; set; }
    public RequestStatus Status { get; set; }
}
