using Hooome.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hooome.Application.CQRS.Requests.Commands.CreateRequest;

public class CreateRequestCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!; 
    public RequestCategory Category { get; set; }
}
