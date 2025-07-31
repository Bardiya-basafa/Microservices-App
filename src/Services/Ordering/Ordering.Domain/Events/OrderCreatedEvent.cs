namespace Ordering.Domain.Events;

using Abstractions;
using Entities;
using MediatR;

public record OrderCreatedEvent(Order Order) : IDomainEvent;
