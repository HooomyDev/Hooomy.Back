using FluentValidation;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;

namespace Hooome.Application.CQRS.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler(IAddressRepository addressRepo)
    : IRequestHandler<CreateAddressCommand, Guid>
{
    public async Task<Guid> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var existingAddress = await addressRepo
            .GetByCoords(request.Latitude, request.Longitude, cancellationToken);

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

        await addressRepo.Create(newAddress, cancellationToken);

        return newAddress.Id;
    }
}
