using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class InventoryConsumedEvent : BaseDomainEvent
    {
        public Inventory Inventory { get; }
        public int ConsumedQuantity { get; }
        public string Reason { get; }
        public string ConsumedBy { get; }

        public InventoryConsumedEvent(Inventory inventory, int consumedQuantity, string reason, string consumedBy) 
            : base("Inventory", inventory.Id.ToString())
        {
            Inventory = inventory;
            ConsumedQuantity = consumedQuantity;
            Reason = reason;
            ConsumedBy = consumedBy;
        }
    }
} 