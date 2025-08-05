using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Events;

namespace DigitekShop.Domain.Entities.Common
{
    public abstract class BaseAggregateRoot : BaseEntity
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public bool HasDomainEvents => _domainEvents.Any();

        protected void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            _domainEvents.Remove(domainEvent);
        }

        protected void RemoveDomainEvents<TEvent>() where TEvent : IDomainEvent
        {
            var eventsToRemove = _domainEvents.OfType<TEvent>().ToList();
            foreach (var domainEvent in eventsToRemove)
            {
                _domainEvents.Remove(domainEvent);
            }
        }
    }
} 