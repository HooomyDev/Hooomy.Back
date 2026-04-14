using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<CreateCompanyCommand, Guid>
{
    public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        if (request.AddressId != Guid.Empty && request.AddressId is not null)
        {
            var exitingAddress = await dbContext.Addresses
                .FindAsync([request.AddressId], cancellationToken)
                ?? throw new NotFoundException(nameof(Address), request.AddressId);
        }

        var newCompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Phone = request.Phone ?? string.Empty,
            Email = request.Email ?? string.Empty,
            WorkingHours = request.WorkingHours ?? string.Empty,
            AddressId = request.AddressId,
            CreatedAt = DateTime.UtcNow,
        };
        
        await dbContext.Companies.AddAsync(newCompany, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newCompany.Id;
    }
}
