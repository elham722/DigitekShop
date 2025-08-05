using DigitekShop.Application.DTOs.Order;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Profiles;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Exceptions;
using DigitekShop.Domain.Services;
using AutoMapper;

namespace DigitekShop.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OrderDomainService _orderDomainService;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            OrderDomainService orderDomainService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _orderDomainService = orderDomainService;
            _mapper = mapper;
        }

        public async Task<OrderDto> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // شروع تراکنش
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                // بررسی وجود مشتری
                var customer = await _unitOfWork.Customers.GetByIdAsync(command.CustomerId);
                if (customer == null)
                    throw new CustomerNotFoundException(command.CustomerId);

                // بررسی فعال بودن مشتری
                if (!customer.IsActive())
                    throw new CustomerNotActiveException(command.CustomerId, "create order");

                // ایجاد آدرس‌ها
                var shippingAddress = new Address(
                    command.ShippingState, // Province
                    command.ShippingCity,  // City
                    "",                    // District
                    command.ShippingStreet, // Street
                    command.ShippingPostalCode, // PostalCode
                    ""                     // Details
                );

                var billingAddress = new Address(
                    command.BillingState,   // Province
                    command.BillingCity,    // City
                    "",                     // District
                    command.BillingStreet,  // Street
                    command.BillingPostalCode, // PostalCode
                    ""                      // Details
                );

                // ایجاد سفارش با استفاده از Order.Create()
                var order = Order.Create(
                    command.CustomerId,
                    shippingAddress,
                    billingAddress,
                    command.PaymentMethod,
                    command.ShippingMethod
                );

                // اضافه کردن یادداشت
                if (!string.IsNullOrEmpty(command.Notes))
                {
                    order.UpdateNotes(command.Notes);
                }

                // ذخیره سفارش اولیه برای دریافت ID
                var createdOrder = await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // اضافه کردن آیتم‌های سفارش و کاهش موجودی
                foreach (var itemDto in command.OrderItems)
                {
                    // بررسی وجود محصول
                    var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                    if (product == null)
                        throw new ProductNotFoundException(itemDto.ProductId);

                    // بررسی موجودی
                    if (!product.IsInStock() || product.StockQuantity < itemDto.Quantity)
                        throw new InsufficientStockException(
                            product.Id, 
                            product.Name.Value, 
                            itemDto.Quantity, 
                            product.StockQuantity);

                    // ایجاد OrderItem
                    var orderItem = new OrderItem(
                        createdOrder.Id,
                        itemDto.ProductId,
                        itemDto.Quantity,
                        new Money(itemDto.UnitPrice, "IRR")
                    );

                    createdOrder.AddOrderItem(orderItem);

                    // کاهش موجودی محصول
                    product.UpdateStock(product.StockQuantity - itemDto.Quantity, "Order creation", "System");
                    
                    // به‌روزرسانی محصول در UnitOfWork
                    await _unitOfWork.Products.UpdateAsync(product);
                }

                // محاسبه هزینه‌های اضافی
                var shippingCost = _orderDomainService.CalculateShippingCost(
                    shippingAddress, 
                    createdOrder.OrderItems, 
                    command.ShippingMethod);
                createdOrder.SetShippingCost(shippingCost);

                // محاسبه مالیات بر اساس کل مبلغ سفارش
                var taxAmount = _orderDomainService.CalculateTaxAmount(createdOrder.TotalAmount);
                createdOrder.SetTaxAmount(taxAmount);

                // ذخیره همه تغییرات
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // تایید تراکنش
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                // تبدیل به DTO و بازگشت
                return _mapper.Map<OrderDto>(createdOrder);
            }
            catch
            {
                // Rollback در صورت خطا
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
} 