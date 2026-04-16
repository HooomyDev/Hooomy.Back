using Hooome.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hooome.Persistance.Extensions;

public static class ModelBuilderExtensions 
{
    public static void ApplySoftDeleteFilters(this ModelBuilder builder)
    {
        var entities = builder.Model.GetEntityTypes()
            .Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType) && !e.ClrType.IsAbstract);

        foreach(var entityType in entities)
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var isDeletedProperty = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            var condition = Expression.Equal(isDeletedProperty, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);

            builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}
