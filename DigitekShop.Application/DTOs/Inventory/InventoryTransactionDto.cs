using DigitekShop.Domain.Enums;

namespace DigitekShop.Application.DTOs.Inventory
{
    public class InventoryTransactionDto
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public InventoryTransactionType Type { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public int QuantityChange { get; set; }
        public string Reason { get; set; }
        public string ChangedBy { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ReferenceNumber { get; set; }
        public bool IsStockIncrease { get; set; }
        public bool IsStockDecrease { get; set; }
    }
} 