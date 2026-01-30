using Hooome.Application.Interfaces;
using Hooome.Domain;

namespace Hooome.WebApi.Services;

public class DataSeedStreetsService(IHooomeDbContext dbContext)
{
   public async Task SeedData(CancellationToken cancellationToken)
    {
        if (dbContext.Streets.Any())
        {
            return;
        }

        var filePath = "streets.txt";

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File {filePath} not found");
        }

        var lines = await File.ReadAllLinesAsync(filePath);

        var streets = new List<Street>();

        foreach (var line in lines)
        {
            var street = new Street
            {
                Id = Guid.NewGuid(),
                Title = line
            };

            streets.Add(street);
        }

        await dbContext.Streets.AddRangeAsync(streets, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
