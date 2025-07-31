namespace Ordering.Domain.Events;

using Abstractions;
using Entities;
using MediatR;

public record OrderUpdatedEvent(Order Order) : IDomainEvent;
