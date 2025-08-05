using DigitekShop.Domain.Entities;

namespace DigitekShop.Domain.Events
{
    public class InventoryStockUpdatedEvent : BaseDomainEvent
    {
        public Inventory Inventory { get; }
        public int OldQuantity { get; }
        public int NewQuantity { get; }
        public string Reason { get; }
        public string ChangedBy { get; }

        public InventoryStockUpdatedEvent(Inventory inventory, int oldQuantity, int newQuantity, string reason, string changedBy) 
            : base("Inventory", inventory.Id.ToString())
        {
            Inventory = inventory;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
            Reason = reason;
            ChangedBy = changedBy;
        }
    }
} 