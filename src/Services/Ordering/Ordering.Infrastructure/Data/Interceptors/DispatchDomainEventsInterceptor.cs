﻿namespace Ordering.Infrastructure.Data.Interceptors;

using Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor {

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null){
            return;
        }

        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);

        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregates.ToList().ForEach(a => a.ClearDomainEvents());

        foreach (var domainEvent in domainEvents){
            await mediator.Publish(domainEvent);
        }
    }

}
