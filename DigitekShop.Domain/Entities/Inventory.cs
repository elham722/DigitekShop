using System;
using System.Collections.Generic;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Events;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Entities
{
    public class Inventory : BaseAggregateRoot
    {
        // Core Properties
        public int ProductId { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public int ReservedQuantity { get; private set; }
        public int AvailableQuantity => Quantity - ReservedQuantity;
        public int MinimumStockLevel { get; private set; }
        public int MaximumStockLevel { get; private set; }
        public string Location { get; private set; }
        public string WarehouseCode { get; private set; }
        public DateTime LastUpdated { get; private set; }
        public InventoryStatus Status { get; private set; }

        // Navigation Properties
        public ICollection<InventoryTransaction> Transactions { get; private set; } = new List<InventoryTransaction>();

        // Constructor
        private Inventory() { } // برای EF Core

        public static Inventory Create(int productId, int initialQuantity, int minimumStockLevel = 10, 
            int maximumStockLevel = 1000, string location = "Main Warehouse", string warehouseCode = "MAIN")
        {
            if (initialQuantity < 0)
                throw new ArgumentException("Initial quantity cannot be negative");

            if (minimumStockLevel < 0)
                throw new ArgumentException("Minimum stock level cannot be negative");

            if (maximumStockLevel <= minimumStockLevel)
                throw new ArgumentException("Maximum stock level must be greater than minimum stock level");

            var inventory = new Inventory
            {
                ProductId = productId,
                Quantity = initialQuantity,
                ReservedQuantity = 0,
                MinimumStockLevel = minimumStockLevel,
                MaximumStockLevel = maximumStockLevel,
                Location = location ?? "Main Warehouse",
                WarehouseCode = warehouseCode ?? "MAIN",
                LastUpdated = DateTime.UtcNow,
                Status = InventoryStatus.Active
            };

            inventory.SetUpdated();
            inventory.AddDomainEvent(new InventoryCreatedEvent(inventory));
            
            return inventory;
        }

        // Business Methods
        public void UpdateStock(int newQuantity, string reason = "", string changedBy = "System")
        {
            if (newQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative");

            if (newQuantity < ReservedQuantity)
                throw new ArgumentException("New quantity cannot be less than reserved quantity");

            var oldQuantity = Quantity;
            Quantity = newQuantity;
            LastUpdated = DateTime.UtcNow;
            UpdateStatus();
            SetUpdated();

            // Add transaction record
            var transaction = InventoryTransaction.Create(
                this.Id, 
                InventoryTransactionType.StockUpdate, 
                oldQuantity, 
                newQuantity, 
                reason, 
                changedBy
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new InventoryStockUpdatedEvent(this, oldQuantity, newQuantity, reason, changedBy));
        }

        public void ReserveStock(int quantity, string reason = "", string reservedBy = "System")
        {
            if (quantity <= 0)
                throw new ArgumentException("Reservation quantity must be positive");

            if (AvailableQuantity < quantity)
                throw new ArgumentException("Insufficient available stock for reservation");

            var oldReservedQuantity = ReservedQuantity;
            ReservedQuantity += quantity;
            LastUpdated = DateTime.UtcNow;
            UpdateStatus();
            SetUpdated();

            // Add transaction record
            var transaction = InventoryTransaction.Create(
                this.Id,
                InventoryTransactionType.Reservation,
                oldReservedQuantity,
                ReservedQuantity,
                reason,
                reservedBy
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new InventoryReservedEvent(this, quantity, reason, reservedBy));
        }

        public void ReleaseReservedStock(int quantity, string reason = "", string releasedBy = "System")
        {
            if (quantity <= 0)
                throw new ArgumentException("Release quantity must be positive");

            if (ReservedQuantity < quantity)
                throw new ArgumentException("Cannot release more than reserved quantity");

            var oldReservedQuantity = ReservedQuantity;
            ReservedQuantity -= quantity;
            LastUpdated = DateTime.UtcNow;
            UpdateStatus();
            SetUpdated();

            // Add transaction record
            var transaction = InventoryTransaction.Create(
                this.Id,
                InventoryTransactionType.Release,
                oldReservedQuantity,
                ReservedQuantity,
                reason,
                releasedBy
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new InventoryReleasedEvent(this, quantity, reason, releasedBy));
        }

        public void ConsumeReservedStock(int quantity, string reason = "", string consumedBy = "System")
        {
            if (quantity <= 0)
                throw new ArgumentException("Consumption quantity must be positive");

            if (ReservedQuantity < quantity)
                throw new ArgumentException("Cannot consume more than reserved quantity");

            var oldReservedQuantity = ReservedQuantity;
            var oldQuantity = Quantity;
            
            ReservedQuantity -= quantity;
            Quantity -= quantity;
            LastUpdated = DateTime.UtcNow;
            UpdateStatus();
            SetUpdated();

            // Add transaction record
            var transaction = InventoryTransaction.Create(
                this.Id,
                InventoryTransactionType.Consumption,
                oldQuantity,
                Quantity,
                reason,
                consumedBy
            );
            Transactions.Add(transaction);

            // Add Domain Event
            AddDomainEvent(new InventoryConsumedEvent(this, quantity, reason, consumedBy));
        }

        public void UpdateMinimumStockLevel(int newMinimumLevel)
        {
            if (newMinimumLevel < 0)
                throw new ArgumentException("Minimum stock level cannot be negative");

            if (newMinimumLevel >= MaximumStockLevel)
                throw new ArgumentException("Minimum stock level must be less than maximum stock level");

            MinimumStockLevel = newMinimumLevel;
            UpdateStatus();
            SetUpdated();
        }

        public void UpdateMaximumStockLevel(int newMaximumLevel)
        {
            if (newMaximumLevel <= MinimumStockLevel)
                throw new ArgumentException("Maximum stock level must be greater than minimum stock level");

            MaximumStockLevel = newMaximumLevel;
            UpdateStatus();
            SetUpdated();
        }

        public void UpdateLocation(string newLocation, string newWarehouseCode = null)
        {
            Location = newLocation ?? Location;
            WarehouseCode = newWarehouseCode ?? WarehouseCode;
            LastUpdated = DateTime.UtcNow;
            SetUpdated();
        }

        public void Deactivate()
        {
            Status = InventoryStatus.Inactive;
            SetUpdated();
        }

        public void Activate()
        {
            Status = InventoryStatus.Active;
            SetUpdated();
        }

        // Query Methods
        public bool IsInStock() => AvailableQuantity > 0 && Status == InventoryStatus.Active;

        public bool IsLowStock() => AvailableQuantity <= MinimumStockLevel && AvailableQuantity > 0;

        public bool IsOutOfStock() => AvailableQuantity == 0;

        public bool IsOverstocked() => Quantity > MaximumStockLevel;

        public bool CanReserve(int quantity) => AvailableQuantity >= quantity && Status == InventoryStatus.Active;

        public decimal GetStockUtilizationPercentage() => 
            MaximumStockLevel > 0 ? (decimal)Quantity / MaximumStockLevel * 100 : 0;

        public int GetDaysUntilStockout(int dailyConsumptionRate)
        {
            if (dailyConsumptionRate <= 0) return int.MaxValue;
            return AvailableQuantity / dailyConsumptionRate;
        }

        // Private Methods
        private void UpdateStatus()
        {
            if (AvailableQuantity == 0)
                Status = InventoryStatus.OutOfStock;
            else if (IsLowStock())
                Status = InventoryStatus.LowStock;
            else if (IsOverstocked())
                Status = InventoryStatus.Overstocked;
            else
                Status = InventoryStatus.Active;
        }
    }
} 