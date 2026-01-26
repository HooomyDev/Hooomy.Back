using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.Requests.Commands.CreateRequest;

public class CreateRequestCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty; 
    public RequestCategory Category { get; set; }
}
