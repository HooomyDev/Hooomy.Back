using Hooome.Application.CQRS.Streets.Queries.GetStreetList;
using Hooome.Domain;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hooome.Application.CQRS.Addresses.Queries.GetAddressList;

public class GetAddressListQuery : IRequest<AddressListVm>
{
    public string Query { get; set; } = string.Empty;
}
