﻿namespace Ordering.Domain.Abstractions;

public class Aggregate<TId> : Entity<TId>, IAggregate<TId> {

    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        var dequeuedDomainEvents = _domainEvents.ToArray();
        _domainEvents.Clear();

        return dequeuedDomainEvents;
    }

}
