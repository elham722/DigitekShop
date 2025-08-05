using System;
using System.Linq.Expressions;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Specifications
{
    public class OrderByCustomerSpecification : BaseSpecification<Order>
    {
        public OrderByCustomerSpecification(int customerId)
        {
            AddCriteria(o => o.CustomerId == customerId);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.CreatedAt);
        }
    }

    public class OrderByStatusSpecification : BaseSpecification<Order>
    {
        public OrderByStatusSpecification(OrderStatus status)
        {
            AddCriteria(o => o.Status == status);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.CreatedAt);
        }
    }

    public class OrderByDateRangeSpecification : BaseSpecification<Order>
    {
        public OrderByDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            AddCriteria(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.CreatedAt);
        }
    }

    public class OrderByAmountRangeSpecification : BaseSpecification<Order>
    {
        public OrderByAmountRangeSpecification(decimal minAmount, decimal maxAmount)
        {
            AddCriteria(o => o.TotalAmount.Amount >= minAmount && o.TotalAmount.Amount <= maxAmount);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.TotalAmount.Amount);
        }
    }

    public class OrderPendingSpecification : BaseSpecification<Order>
    {
        public OrderPendingSpecification()
        {
            AddCriteria(o => o.Status == OrderStatus.Pending);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderBy(o => o.CreatedAt);
        }
    }

    public class OrderConfirmedSpecification : BaseSpecification<Order>
    {
        public OrderConfirmedSpecification()
        {
            AddCriteria(o => o.Status == OrderStatus.Confirmed);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderBy(o => o.CreatedAt);
        }
    }

    public class OrderProcessingSpecification : BaseSpecification<Order>
    {
        public OrderProcessingSpecification()
        {
            AddCriteria(o => o.Status == OrderStatus.Processing);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderBy(o => o.CreatedAt);
        }
    }

    public class OrderShippedSpecification : BaseSpecification<Order>
    {
        public OrderShippedSpecification()
        {
            AddCriteria(o => o.Status == OrderStatus.Shipped);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderBy(o => o.ShippedAt);
        }
    }

    public class OrderDeliveredSpecification : BaseSpecification<Order>
    {
        public OrderDeliveredSpecification()
        {
            AddCriteria(o => o.Status == OrderStatus.Delivered);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.DeliveredAt);
        }
    }

    public class OrderCancelledSpecification : BaseSpecification<Order>
    {
        public OrderCancelledSpecification()
        {
            AddCriteria(o => o.Status == OrderStatus.Cancelled);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.UpdatedAt);
        }
    }

    public class OrderOverdueSpecification : BaseSpecification<Order>
    {
        public OrderOverdueSpecification()
        {
            AddCriteria(o => o.EstimatedDeliveryDate.HasValue && 
                             o.EstimatedDeliveryDate.Value < DateTime.UtcNow && 
                             o.Status != OrderStatus.Delivered && 
                             o.Status != OrderStatus.Cancelled);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderBy(o => o.EstimatedDeliveryDate);
        }
    }

    public class OrderWithTrackingNumberSpecification : BaseSpecification<Order>
    {
        public OrderWithTrackingNumberSpecification()
        {
            AddCriteria(o => !string.IsNullOrEmpty(o.TrackingNumber));
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.CreatedAt);
        }
    }

    public class OrderByPaymentMethodSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentMethodSpecification(PaymentMethod paymentMethod)
        {
            AddCriteria(o => o.PaymentMethod == paymentMethod);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.CreatedAt);
        }
    }

    public class OrderWithDiscountSpecification : BaseSpecification<Order>
    {
        public OrderWithDiscountSpecification()
        {
            AddCriteria(o => o.DiscountAmount.Amount > 0);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.DiscountAmount.Amount);
        }
    }

    public class OrderHighValueSpecification : BaseSpecification<Order>
    {
        public OrderHighValueSpecification(decimal threshold = 1000000m)
        {
            AddCriteria(o => o.TotalAmount.Amount >= threshold);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.TotalAmount.Amount);
        }
    }

    public class OrderRecentSpecification : BaseSpecification<Order>
    {
        public OrderRecentSpecification(int days = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            AddCriteria(o => o.CreatedAt >= cutoffDate);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.CreatedAt);
        }
    }

    public class OrderWithPagingSpecification : BaseSpecification<Order>
    {
        public OrderWithPagingSpecification(int page, int pageSize, OrderStatus? status = null, int? customerId = null)
        {
            var skip = (page - 1) * pageSize;
            ApplyPaging(skip, pageSize);

            Expression<Func<Order, bool>> criteria = o => true;

            if (status.HasValue)
            {
                criteria = o => o.Status == status.Value;
            }

            if (customerId.HasValue)
            {
                criteria = o => o.CustomerId == customerId.Value;
            }

            if (status.HasValue && customerId.HasValue)
            {
                criteria = o => o.Status == status.Value && o.CustomerId == customerId.Value;
            }

            AddCriteria(criteria);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.CreatedAt);
        }
    }

    public class OrderByOrderNumberSpecification : BaseSpecification<Order>
    {
        public OrderByOrderNumberSpecification(string orderNumber)
        {
            AddCriteria(o => o.OrderNumber.Value == orderNumber);
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
        }
    }

    public class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification()
        {
            AddCriteria(o => o.OrderItems.Any());
            AddInclude(o => o.Customer);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.CreatedAt);
        }
    }
} 