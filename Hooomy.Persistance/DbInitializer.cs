using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance;

public class DbInitializer
{
    public static void Initialize(HooomeDbContext context)
    {
        context.Database.Migrate();
    }
}