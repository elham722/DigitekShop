using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class InventoryReservedEvent : BaseDomainEvent
    {
        public Inventory Inventory { get; }
        public int ReservedQuantity { get; }
        public string Reason { get; }
        public string ReservedBy { get; }

        public InventoryReservedEvent(Inventory inventory, int reservedQuantity, string reason, string reservedBy) 
            : base("Inventory", inventory.Id.ToString())
        {
            Inventory = inventory;
            ReservedQuantity = reservedQuantity;
            Reason = reason;
            ReservedBy = reservedBy;
        }
    }
} 