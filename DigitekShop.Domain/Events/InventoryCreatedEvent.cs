using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class InventoryCreatedEvent : BaseDomainEvent
    {
        public Inventory Inventory { get; }

        public InventoryCreatedEvent(Inventory inventory) : base("Inventory", inventory.Id.ToString())
        {
            Inventory = inventory;
        }
    }
} 