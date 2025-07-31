using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.BusinessRules
{
    public class OrderBusinessRules
    {
        public class OrderMustHaveItemsRule : IBusinessRule
        {
            private readonly ICollection<OrderItem> _orderItems;

            public OrderMustHaveItemsRule(ICollection<OrderItem> orderItems)
            {
                _orderItems = orderItems;
            }

            public bool IsBroken() => !_orderItems.Any();

            public string Message => "Order must have at least one item";
        }

        public class OrderTotalMustBePositiveRule : IBusinessRule
        {
            private readonly Money _totalAmount;

            public OrderTotalMustBePositiveRule(Money totalAmount)
            {
                _totalAmount = totalAmount;
            }

            public bool IsBroken() => _totalAmount.Amount <= 0;

            public string Message => "Order total amount must be greater than zero";
        }

        public class CustomerMustBeActiveRule : IBusinessRule
        {
            private readonly Customer _customer;

            public CustomerMustBeActiveRule(Customer customer)
            {
                _customer = customer;
            }

            public bool IsBroken() => !_customer.IsActive();

            public string Message => "Customer must be active to create an order";
        }

        public class CustomerMustNotBeBlockedRule : IBusinessRule
        {
            private readonly Customer _customer;

            public CustomerMustNotBeBlockedRule(Customer customer)
            {
                _customer = customer;
            }

            public bool IsBroken() => _customer.IsBlocked();

            public string Message => "Blocked customers cannot create orders";
        }

        public class OrderItemsMustBeInStockRule : IBusinessRule
        {
            private readonly ICollection<OrderItem> _orderItems;

            public OrderItemsMustBeInStockRule(ICollection<OrderItem> orderItems)
            {
                _orderItems = orderItems;
            }

            public bool IsBroken()
            {
                return _orderItems.Any(item => !item.Product.IsInStock());
            }

            public string Message => "All order items must be in stock";
        }

        public class OrderItemsMustHaveSufficientStockRule : IBusinessRule
        {
            private readonly ICollection<OrderItem> _orderItems;

            public OrderItemsMustHaveSufficientStockRule(ICollection<OrderItem> orderItems)
            {
                _orderItems = orderItems;
            }

            public bool IsBroken()
            {
                return _orderItems.Any(item => item.Quantity > item.Product.StockQuantity);
            }

            public string Message => "Insufficient stock for one or more items";
        }

        public class OrderDiscountCannotExceedLimitRule : IBusinessRule
        {
            private readonly Money _discountAmount;
            private readonly Money _totalAmount;
            private readonly decimal _maxDiscountPercentage;

            public OrderDiscountCannotExceedLimitRule(Money discountAmount, Money totalAmount, decimal maxDiscountPercentage = 30)
            {
                _discountAmount = discountAmount;
                _totalAmount = totalAmount;
                _maxDiscountPercentage = maxDiscountPercentage;
            }

            public bool IsBroken()
            {
                if (_totalAmount.Amount == 0) return false;
                var discountPercentage = (_discountAmount.Amount / _totalAmount.Amount) * 100;
                return discountPercentage > _maxDiscountPercentage;
            }

            public string Message => $"Discount cannot exceed {_maxDiscountPercentage}% of total amount";
        }

        public class OrderStatusTransitionMustBeValidRule : IBusinessRule
        {
            private readonly OrderStatus _currentStatus;
            private readonly OrderStatus _newStatus;

            public OrderStatusTransitionMustBeValidRule(OrderStatus currentStatus, OrderStatus newStatus)
            {
                _currentStatus = currentStatus;
                _newStatus = newStatus;
            }

            public bool IsBroken()
            {
                return !IsValidTransition(_currentStatus, _newStatus);
            }

            public string Message => $"Cannot change order status from {_currentStatus} to {_newStatus}";

            private bool IsValidTransition(OrderStatus currentStatus, OrderStatus newStatus)
            {
                return (currentStatus, newStatus) switch
                {
                    (OrderStatus.Pending, OrderStatus.Confirmed) => true,
                    (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                    (OrderStatus.Confirmed, OrderStatus.Processing) => true,
                    (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
                    (OrderStatus.Processing, OrderStatus.Shipped) => true,
                    (OrderStatus.Shipped, OrderStatus.Delivered) => true,
                    (OrderStatus.Delivered, OrderStatus.Refunded) => true,
                    (OrderStatus.Delivered, OrderStatus.Returned) => true,
                    _ => false
                };
            }
        }

        public class OrderMustHaveValidAddressesRule : IBusinessRule
        {
            private readonly Address _shippingAddress;
            private readonly Address _billingAddress;

            public OrderMustHaveValidAddressesRule(Address shippingAddress, Address billingAddress)
            {
                _shippingAddress = shippingAddress;
                _billingAddress = billingAddress;
            }

            public bool IsBroken()
            {
                return _shippingAddress == null || _billingAddress == null ||
                       !_shippingAddress.IsComplete() || !_billingAddress.IsComplete();
            }

            public string Message => "Order must have valid shipping and billing addresses";
        }

        public class OrderCannotBeModifiedAfterDeliveryRule : IBusinessRule
        {
            private readonly OrderStatus _status;

            public OrderCannotBeModifiedAfterDeliveryRule(OrderStatus status)
            {
                _status = status;
            }

            public bool IsBroken()
            {
                return _status == OrderStatus.Delivered || 
                       _status == OrderStatus.Refunded || 
                       _status == OrderStatus.Returned;
            }

            public string Message => "Order cannot be modified after delivery";
        }
    }
} 