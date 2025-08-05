using System;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Entities
{
    public class InventoryTransaction : BaseEntity
    {
        // Core Properties
        public int InventoryId { get; private set; }
        public Inventory Inventory { get; private set; }
        public InventoryTransactionType Type { get; private set; }
        public int OldQuantity { get; private set; }
        public int NewQuantity { get; private set; }
        public int QuantityChange => NewQuantity - OldQuantity;
        public string Reason { get; private set; }
        public string ChangedBy { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string ReferenceNumber { get; private set; }

        // Constructor
        private InventoryTransaction() { } // برای EF Core

        public static InventoryTransaction Create(int inventoryId, InventoryTransactionType type, 
            int oldQuantity, int newQuantity, string reason = "", string changedBy = "System")
        {
            var transaction = new InventoryTransaction
            {
                InventoryId = inventoryId,
                Type = type,
                OldQuantity = oldQuantity,
                NewQuantity = newQuantity,
                Reason = reason ?? "",
                ChangedBy = changedBy ?? "System",
                TransactionDate = DateTime.UtcNow,
                ReferenceNumber = GenerateReferenceNumber()
            };

            transaction.SetUpdated();
            return transaction;
        }

        // Private Methods
        private static string GenerateReferenceNumber()
        {
            return $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        // Query Methods
        public bool IsStockIncrease() => QuantityChange > 0;
        public bool IsStockDecrease() => QuantityChange < 0;
        public bool IsStockUpdate() => Type == InventoryTransactionType.StockUpdate;
        public bool IsReservation() => Type == InventoryTransactionType.Reservation;
        public bool IsRelease() => Type == InventoryTransactionType.Release;
        public bool IsConsumption() => Type == InventoryTransactionType.Consumption;
    }
} 