using Hooome.Domain.Enums;
using MediatR;

namespace Hooome.Application.Works.Commands.CreateWork;

public class CreateWorkCommand : IRequest<Guid>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Street { get; set; } = null!;
    public int House { get; set; }
    public RequestCategory Category { get; set; }
    public WorkSeriousness Seriousness { get; set; }
    public DateTime PlannedStartTime { get; set; }
    public DateTime PlannedEndTime { get; set; }
}
