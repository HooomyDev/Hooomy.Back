using Hooome.Application.Interfaces;
using Hooome.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Requests.Queries.GetMapData;

public class GetMapDataQueryHandler(
    IHooomeDbContext context,
    IMapClusteringService clusteringService) 
    : IRequestHandler<GetMapDataQuery, MapDataVm>
{
    public async Task<MapDataVm> Handle(GetMapDataQuery request, CancellationToken cancellationToken)
    {
        var addresses = await GetAddressesWithRequests(request, cancellationToken);

        var cellSize = GetCellSizeByZoom(request.ZoomLevel);

        var clusters = clusteringService.ClusterAddresses(addresses, cellSize);

        return new MapDataVm
        {
            Clusters = clusters,
            ZoomLevel = request.ZoomLevel,
            TotalRequests = addresses.Sum(a => a.RequestsCount),
            TotalAddresses = addresses.Count
        };
    }

    private async Task<List<AddressMapDto>> GetAddressesWithRequests(
        GetMapDataQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Requests
            .Include(r => r.Address)
            .Where(r => r.Address.Latitude != null && r.Address.Longitude != null)
            .AsQueryable();

        if (request.Month > 0)
        {
            query = query.Where(r => r.CreatedAt.Month == request.Month && r.CreatedAt.Year == DateTime.Now.Year);
        }

        query = request.RequestStatus switch
        {
            RequestStatus.Created => query.Where(r => r.Status == RequestStatus.Created),
            RequestStatus.Completed => query.Where(r => r.Status == RequestStatus.Completed),
            RequestStatus.InProgress => query.Where(r => r.Status == RequestStatus.InProgress),
            _ => query
        };

        var addresses = await query
            .GroupBy(r => r.AddressId)
            .Select(g => new AddressMapDto
            {
                Id = g.Key,
                Street = g.First().Address.Street,
                HouseNumber = g.First().Address.HouseNumber,
                Latitude = (double)g.First().Address.Latitude!,
                Longitude = (double)g.First().Address.Longitude!,
                RequestsCount = g.Count(),
            })
            .ToListAsync(cancellationToken);

        return addresses;
    }

    private static double GetCellSizeByZoom(int zoomLevel) => zoomLevel switch
    {
        <= 10 => 0.05,    // ~5.5 км - районы
        <= 12 => 0.02,    // ~2.2 км - микрорайоны
        <= 14 => 0.01,    // ~1.1 км - кварталы
        <= 16 => 0.005,   // ~550 м - улицы
        <= 18 => 0.002,   // ~220 м - дома
        _ => 0.001        // ~110 м - отдельные адреса
    };
}