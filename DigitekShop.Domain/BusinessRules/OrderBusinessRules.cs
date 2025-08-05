using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.BusinessRules
{
    public class OrderMustHaveItemsRule : BaseBusinessRule
    {
        private readonly ICollection<OrderItem> _orderItems;

        public OrderMustHaveItemsRule(ICollection<OrderItem> orderItems)
        {
            _orderItems = orderItems ?? throw new ArgumentNullException(nameof(orderItems));
        }

        public override bool IsBroken() => !_orderItems.Any();

        public override string Message => "سفارش باید حداقل یک آیتم داشته باشد";

        public override string RuleName => "OrderMustHaveItems";

        public override string ErrorCode => "ORDER_001";

        public override bool IsCritical => true;
    }

    public class OrderTotalAmountMustBePositiveRule : BaseBusinessRule
    {
        private readonly Money _totalAmount;

        public OrderTotalAmountMustBePositiveRule(Money totalAmount)
        {
            _totalAmount = totalAmount;
        }

        public override bool IsBroken() => _totalAmount.Amount <= 0;

        public override string Message => "مبلغ کل سفارش باید بیشتر از صفر باشد";

        public override string RuleName => "OrderTotalAmountMustBePositive";

        public override string ErrorCode => "ORDER_002";

        public override bool IsCritical => true;
    }

    public class OrderCanBeCancelledRule : BaseBusinessRule
    {
        private readonly OrderStatus _currentStatus;

        public OrderCanBeCancelledRule(OrderStatus currentStatus)
        {
            _currentStatus = currentStatus;
        }

        public override bool IsBroken() => !CanBeCancelled(_currentStatus);

        public override string Message => "سفارش در این وضعیت قابل لغو نیست";

        public override string RuleName => "OrderCanBeCancelled";

        public override string ErrorCode => "ORDER_003";

        public override bool IsCritical => false;

        private bool CanBeCancelled(OrderStatus status)
        {
            return status == OrderStatus.Pending || status == OrderStatus.Confirmed;
        }
    }

    public class OrderDiscountCannotExceedTotalRule : BaseBusinessRule
    {
        private readonly Money _discountAmount;
        private readonly Money _totalAmount;

        public OrderDiscountCannotExceedTotalRule(Money discountAmount, Money totalAmount)
        {
            _discountAmount = discountAmount;
            _totalAmount = totalAmount;
        }

        public override bool IsBroken() => _discountAmount.Amount > _totalAmount.Amount;

        public override string Message => "مبلغ تخفیف نمی‌تواند بیشتر از مبلغ کل باشد";

        public override string RuleName => "OrderDiscountCannotExceedTotal";

        public override string ErrorCode => "ORDER_004";

        public override bool IsCritical => true;
    }

    public class OrderMustHaveValidAddressesRule : BaseBusinessRule
    {
        private readonly Address _shippingAddress;
        private readonly Address _billingAddress;

        public OrderMustHaveValidAddressesRule(Address shippingAddress, Address billingAddress)
        {
            _shippingAddress = shippingAddress;
            _billingAddress = billingAddress;
        }

        public override bool IsBroken() => _shippingAddress == null || _billingAddress == null;

        public override string Message => "سفارش باید آدرس ارسال و صورتحساب معتبر داشته باشد";

        public override string RuleName => "OrderMustHaveValidAddresses";

        public override string ErrorCode => "ORDER_005";

        public override bool IsCritical => true;
    }

    public class OrderItemQuantityMustBePositiveRule : BaseBusinessRule
    {
        private readonly int _quantity;

        public OrderItemQuantityMustBePositiveRule(int quantity)
        {
            _quantity = quantity;
        }

        public override bool IsBroken() => _quantity <= 0;

        public override string Message => "تعداد آیتم سفارش باید بیشتر از صفر باشد";

        public override string RuleName => "OrderItemQuantityMustBePositive";

        public override string ErrorCode => "ORDER_006";

        public override bool IsCritical => true;
    }

    public class OrderItemPriceMustBePositiveRule : BaseBusinessRule
    {
        private readonly Money _price;

        public OrderItemPriceMustBePositiveRule(Money price)
        {
            _price = price;
        }

        public override bool IsBroken() => _price.Amount <= 0;

        public override string Message => "قیمت آیتم سفارش باید بیشتر از صفر باشد";

        public override string RuleName => "OrderItemPriceMustBePositive";

        public override string ErrorCode => "ORDER_007";

        public override bool IsCritical => true;
    }
} 