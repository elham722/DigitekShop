using System;

namespace DigitekShop.Domain.Events
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        public Guid EventId { get; protected set; }
        public DateTime OccurredOn { get; protected set; }
        public string EventType { get; protected set; }
        public string AggregateType { get; protected set; }
        public string AggregateId { get; protected set; }

        protected BaseDomainEvent()
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            EventType = GetType().Name;
        }

        protected BaseDomainEvent(string aggregateType, string aggregateId) : this()
        {
            AggregateType = aggregateType;
            AggregateId = aggregateId;
        }
    }
} 