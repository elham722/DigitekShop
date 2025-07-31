using System;
using System.Linq.Expressions;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Specifications
{
    public class OrderSpecifications
    {
        public class OrdersByCustomerSpecification : BaseSpecification<Order>
        {
            public OrdersByCustomerSpecification(int customerId)
            {
                AddCriteria(o => o.CustomerId == customerId);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.CreatedAt);
            }
        }

        public class OrdersByStatusSpecification : BaseSpecification<Order>
        {
            public OrdersByStatusSpecification(OrderStatus status)
            {
                AddCriteria(o => o.Status == status);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.CreatedAt);
            }
        }

        public class PendingOrdersSpecification : BaseSpecification<Order>
        {
            public PendingOrdersSpecification()
            {
                AddCriteria(o => o.Status == OrderStatus.Pending);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderBy(o => o.CreatedAt);
            }
        }

        public class ProcessingOrdersSpecification : BaseSpecification<Order>
        {
            public ProcessingOrdersSpecification()
            {
                AddCriteria(o => o.Status == OrderStatus.Processing);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderBy(o => o.CreatedAt);
            }
        }

        public class ShippedOrdersSpecification : BaseSpecification<Order>
        {
            public ShippedOrdersSpecification()
            {
                AddCriteria(o => o.Status == OrderStatus.Shipped);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.ShippedAt);
            }
        }

        public class DeliveredOrdersSpecification : BaseSpecification<Order>
        {
            public DeliveredOrdersSpecification()
            {
                AddCriteria(o => o.Status == OrderStatus.Delivered);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.DeliveredAt);
            }
        }

        public class CancelledOrdersSpecification : BaseSpecification<Order>
        {
            public CancelledOrdersSpecification()
            {
                AddCriteria(o => o.Status == OrderStatus.Cancelled);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.UpdatedAt);
            }
        }

        public class OrdersByDateRangeSpecification : BaseSpecification<Order>
        {
            public OrdersByDateRangeSpecification(DateTime startDate, DateTime endDate)
            {
                AddCriteria(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.CreatedAt);
            }
        }

        public class HighValueOrdersSpecification : BaseSpecification<Order>
        {
            public HighValueOrdersSpecification(decimal minAmount = 1000000m)
            {
                AddCriteria(o => o.TotalAmount.Amount >= minAmount);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.TotalAmount.Amount);
            }
        }

        public class OrdersWithTrackingSpecification : BaseSpecification<Order>
        {
            public OrdersWithTrackingSpecification()
            {
                AddCriteria(o => !string.IsNullOrEmpty(o.TrackingNumber));
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.ShippedAt);
            }
        }

        public class OverdueOrdersSpecification : BaseSpecification<Order>
        {
            public OverdueOrdersSpecification()
            {
                AddCriteria(o => o.EstimatedDeliveryDate.HasValue && 
                                o.EstimatedDeliveryDate < DateTime.UtcNow && 
                                o.Status != OrderStatus.Delivered && 
                                o.Status != OrderStatus.Cancelled);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderBy(o => o.EstimatedDeliveryDate);
            }
        }

        public class OrdersByPaymentMethodSpecification : BaseSpecification<Order>
        {
            public OrdersByPaymentMethodSpecification(PaymentMethod paymentMethod)
            {
                AddCriteria(o => o.PaymentMethod == paymentMethod);
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.CreatedAt);
            }
        }

        public class OrdersWithPagingSpecification : BaseSpecification<Order>
        {
            public OrdersWithPagingSpecification(int page, int pageSize)
            {
                AddInclude(o => o.Customer);
                AddInclude(o => o.OrderItems);
                AddOrderByDescending(o => o.CreatedAt);
                ApplyPaging((page - 1) * pageSize, pageSize);
            }
        }
    }
} 