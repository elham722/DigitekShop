using DigitekShop.Domain.BusinessRules;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.BusinessRules
{
    public class InventoryBusinessRules
    {
        public class MinimumStockLevelRule : BaseBusinessRule
        {
            private readonly Inventory _inventory;
            private readonly int _newMinimumLevel;

            public MinimumStockLevelRule(Inventory inventory, int newMinimumLevel)
            {
                _inventory = inventory;
                _newMinimumLevel = newMinimumLevel;
            }

            public override bool IsBroken()
            {
                return _newMinimumLevel < 0 || _newMinimumLevel >= _inventory.MaximumStockLevel;
            }

            public override string Message => "Minimum stock level must be non-negative and less than maximum stock level";

            public override string RuleName => "MinimumStockLevelRule";

            public override string ErrorCode => "INV001";

            public override int Priority => 1;

            public override bool IsCritical => true;
        }

        public class MaximumStockLevelRule : BaseBusinessRule
        {
            private readonly Inventory _inventory;
            private readonly int _newMaximumLevel;

            public MaximumStockLevelRule(Inventory inventory, int newMaximumLevel)
            {
                _inventory = inventory;
                _newMaximumLevel = newMaximumLevel;
            }

            public override bool IsBroken()
            {
                return _newMaximumLevel <= _inventory.MinimumStockLevel;
            }

            public override string Message => "Maximum stock level must be greater than minimum stock level";

            public override string RuleName => "MaximumStockLevelRule";

            public override string ErrorCode => "INV002";

            public override int Priority => 1;

            public override bool IsCritical => true;
        }

        public class StockReservationRule : BaseBusinessRule
        {
            private readonly Inventory _inventory;
            private readonly int _quantityToReserve;

            public StockReservationRule(Inventory inventory, int quantityToReserve)
            {
                _inventory = inventory;
                _quantityToReserve = quantityToReserve;
            }

            public override bool IsBroken()
            {
                return _quantityToReserve <= 0 || !_inventory.CanReserve(_quantityToReserve);
            }

            public override string Message
            {
                get
                {
                    if (_quantityToReserve <= 0)
                        return "Reservation quantity must be positive";
                    return "Insufficient available stock for reservation";
                }
            }

            public override string RuleName => "StockReservationRule";

            public override string ErrorCode => "INV003";

            public override int Priority => 1;

            public override bool IsCritical => true;
        }

        public class StockReleaseRule : BaseBusinessRule
        {
            private readonly Inventory _inventory;
            private readonly int _quantityToRelease;

            public StockReleaseRule(Inventory inventory, int quantityToRelease)
            {
                _inventory = inventory;
                _quantityToRelease = quantityToRelease;
            }

            public override bool IsBroken()
            {
                return _quantityToRelease <= 0 || _inventory.ReservedQuantity < _quantityToRelease;
            }

            public override string Message
            {
                get
                {
                    if (_quantityToRelease <= 0)
                        return "Release quantity must be positive";
                    return "Cannot release more than reserved quantity";
                }
            }

            public override string RuleName => "StockReleaseRule";

            public override string ErrorCode => "INV004";

            public override int Priority => 1;

            public override bool IsCritical => true;
        }

        public class StockConsumptionRule : BaseBusinessRule
        {
            private readonly Inventory _inventory;
            private readonly int _quantityToConsume;

            public StockConsumptionRule(Inventory inventory, int quantityToConsume)
            {
                _inventory = inventory;
                _quantityToConsume = quantityToConsume;
            }

            public override bool IsBroken()
            {
                return _quantityToConsume <= 0 || _inventory.ReservedQuantity < _quantityToConsume;
            }

            public override string Message
            {
                get
                {
                    if (_quantityToConsume <= 0)
                        return "Consumption quantity must be positive";
                    return "Cannot consume more than reserved quantity";
                }
            }

            public override string RuleName => "StockConsumptionRule";

            public override string ErrorCode => "INV005";

            public override int Priority => 1;

            public override bool IsCritical => true;
        }

        public class StockUpdateRule : BaseBusinessRule
        {
            private readonly Inventory _inventory;
            private readonly int _newQuantity;

            public StockUpdateRule(Inventory inventory, int newQuantity)
            {
                _inventory = inventory;
                _newQuantity = newQuantity;
            }

            public override bool IsBroken()
            {
                return _newQuantity < 0 || _newQuantity < _inventory.ReservedQuantity;
            }

            public override string Message
            {
                get
                {
                    if (_newQuantity < 0)
                        return "Stock quantity cannot be negative";
                    return "New quantity cannot be less than reserved quantity";
                }
            }

            public override string RuleName => "StockUpdateRule";

            public override string ErrorCode => "INV006";

            public override int Priority => 1;

            public override bool IsCritical => true;
        }

        public class LowStockAlertRule : BaseBusinessRule
        {
            private readonly Inventory _inventory;

            public LowStockAlertRule(Inventory inventory)
            {
                _inventory = inventory;
            }

            public override bool IsBroken()
            {
                return _inventory.IsLowStock();
            }

            public override string Message => $"Low stock alert: Product {_inventory.ProductId} has only {_inventory.AvailableQuantity} items available";

            public override string RuleName => "LowStockAlertRule";

            public override string ErrorCode => "INV007";

            public override int Priority => 2;

            public override bool IsCritical => false;
        }

        public class OverstockAlertRule : BaseBusinessRule
        {
            private readonly Inventory _inventory;

            public OverstockAlertRule(Inventory inventory)
            {
                _inventory = inventory;
            }

            public override bool IsBroken()
            {
                return _inventory.IsOverstocked();
            }

            public override string Message => $"Overstock alert: Product {_inventory.ProductId} has {_inventory.Quantity} items (max: {_inventory.MaximumStockLevel})";

            public override string RuleName => "OverstockAlertRule";

            public override string ErrorCode => "INV008";

            public override int Priority => 2;

            public override bool IsCritical => false;
        }
    }
} 