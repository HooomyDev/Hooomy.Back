using Hooome.Application.CQRS.Requests.Queries.GetMapData;
using Hooome.Application.Interfaces;

namespace Hooome.WebApi.Services;

public class MapClusteringService : IMapClusteringService
{
    private const double DefaultCellSize = 0.005; 

    public List<GridCluster> ClusterAddresses(List<AddressMapDto> addresses, double? cellSize = null)
    {
        var size = cellSize ?? DefaultCellSize;

        var clusters = addresses
            .GroupBy(a => GetCellKey(a.Latitude, a.Longitude, size))
            .Select(g => new GridCluster
            {
                Id = Guid.NewGuid(),
                CellX = g.Key.CellX,
                CellY = g.Key.CellY,
                Latitude = g.Key.CenterLat,
                Longitude = g.Key.CenterLng,
                AddressesCount = g.Count(),
                TotalRequests = g.Sum(a => a.RequestsCount),
                Addresses = g.Take(10).ToList(), // Первые 10 адресов для превью
                Bounds = GetCellBounds(g.Key.CellX, g.Key.CellY, size)
            })
            .OrderByDescending(c => c.TotalRequests)
            .ToList();

        return clusters;
    }

    /// <summary>
    /// Получить ключ ячейки для координат
    /// </summary>
    private (int CellX, int CellY, double CenterLat, double CenterLng)
        GetCellKey(double lat, double lng, double cellSize)
    {
        // Вычисляем индексы ячейки
        var cellX = (int)Math.Floor(lng / cellSize);
        var cellY = (int)Math.Floor(lat / cellSize);

        // Вычисляем центр ячейки
        var centerLng = (cellX + 0.5) * cellSize;
        var centerLat = (cellY + 0.5) * cellSize;

        return (cellX, cellY, centerLat, centerLng);
    }

    /// <summary>
    /// Получить границы ячейки
    /// </summary>
    private static (double MinLat, double MaxLat, double MinLng, double MaxLng)
        GetCellBounds(int cellX, int cellY, double cellSize)
    {
        return (
            MinLng: cellX * cellSize,
            MaxLng: (cellX + 1) * cellSize,
            MinLat: cellY * cellSize,
            MaxLat: (cellY + 1) * cellSize
        );
    }
}

