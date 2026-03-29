using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.Requests.Commands.CreateRequest;

public class CreateRequestCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!; 
    public RequestCategory Category { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
}
