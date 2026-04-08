using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hooome.Application.CQRS.Companies.Commands.UploadLogo;

public class UploadLogoCommand : IRequest
{
    public Guid CompanyId { get; set; }
    public IFormFile File { get; set; } = null!;
}
