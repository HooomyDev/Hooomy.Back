using FluentValidation;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler(IHooomeDbContext dbContext)
    : IRequestHandler<CreateAddressCommand, Guid>
{
    public async Task<Guid> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var existingAddress = await dbContext.Addresses
             .FirstOrDefaultAsync(a =>
                 a.Latitude == (decimal)request.Latitude &&
                 a.Longitude == (decimal)request.Longitude, cancellationToken);

        if (existingAddress is not null)
            return existingAddress.Id;

        var addressParts = request.Address.Split(',');

        if (addressParts.Length != 2)
            throw new ValidationException("Address must be like \"Street, HouseNumber\"");

        var newAddress = new Address
        {
            Id = Guid.NewGuid(),
            Street = addressParts[0],
            HouseNumber = addressParts[1] ?? "",
            Latitude = (decimal)request.Latitude,
            Longitude = (decimal)request.Longitude
        };

        await dbContext.Addresses.AddAsync(newAddress, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return newAddress.Id;
    }
}
