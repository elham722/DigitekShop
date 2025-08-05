using System;

namespace DigitekShop.Domain.Events
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        public Guid EventId { get; }
        public DateTime OccurredOn { get; }
        public string EventType { get; }
        public string AggregateType { get; }
        public string AggregateId { get; }

        protected BaseDomainEvent(string aggregateType, string aggregateId)
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            EventType = GetType().Name;
            AggregateType = aggregateType;
            AggregateId = aggregateId;
        }
    }
} 