using System;

namespace DigitekShop.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
} 