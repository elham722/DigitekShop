using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class InventoryReleasedEvent : BaseDomainEvent
    {
        public Inventory Inventory { get; }
        public int ReleasedQuantity { get; }
        public string Reason { get; }
        public string ReleasedBy { get; }

        public InventoryReleasedEvent(Inventory inventory, int releasedQuantity, string reason, string releasedBy) 
            : base("Inventory", inventory.Id.ToString())
        {
            Inventory = inventory;
            ReleasedQuantity = releasedQuantity;
            Reason = reason;
            ReleasedBy = releasedBy;
        }
    }
} 