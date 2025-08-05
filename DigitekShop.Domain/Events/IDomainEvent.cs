using System;

namespace DigitekShop.Domain.Events
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
        string EventType { get; }
        string AggregateType { get; }
        string AggregateId { get; }
    }
} 