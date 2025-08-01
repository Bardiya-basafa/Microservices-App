﻿namespace Ordering.Infrastructure.Data.Interceptors;

using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;


public class AuditableEntityInterceptor : SaveChangesInterceptor {

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateEntities(DbContext? context)
    {
        if (context == null){
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<IEntity>()){
            if (entry.State == EntityState.Added){
                entry.Entity.CreatedBy = "Bardiya";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities()){
                entry.Entity.LastModifiedBy = "Bardiya";
                entry.Entity.LastModified = DateTime.UtcNow;
            }
        }
    }

}


public static class Extensions {

    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r => r.TargetEntry != null && r.TargetEntry.Metadata.IsOwned() && (r.TargetEntry.State == EntityState.Modified));
    }

}
