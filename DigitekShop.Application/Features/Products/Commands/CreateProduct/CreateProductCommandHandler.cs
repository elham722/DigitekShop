using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Profiles;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using AutoMapper;

namespace DigitekShop.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // شروع تراکنش
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                // بررسی وجود دسته‌بندی
                var category = await _unitOfWork.Categories.GetByIdAsync(command.CategoryId);
                if (category == null)
                    throw new ArgumentException($"Category with ID {command.CategoryId} not found");

                // بررسی وجود برند (اگر مشخص شده)
                if (command.BrandId.HasValue)
                {
                    var brand = await _unitOfWork.Brands.GetByIdAsync(command.BrandId.Value);
                    if (brand == null)
                        throw new ArgumentException($"Brand with ID {command.BrandId.Value} not found");
                }

                // بررسی تکراری نبودن SKU
                var existingProduct = await _unitOfWork.Products.GetBySKUAsync(new SKU(command.SKU));
                if (existingProduct != null)
                    throw new ArgumentException($"Product with SKU {command.SKU} already exists");

                // ایجاد Value Objects
                var productName = new ProductName(command.Name);
                var price = new Money(command.Price);
                var sku = new SKU(command.SKU);

                // ایجاد محصول جدید
                var product = new Product(
                    productName,
                    command.Description,
                    price,
                    command.StockQuantity,
                    sku,
                    command.CategoryId,
                    command.BrandId,
                    command.Model,
                    command.Weight
                );

                // تنظیم تصویر
                if (!string.IsNullOrEmpty(command.ImageUrl))
                {
                    product.UpdateImageUrl(command.ImageUrl);
                }

                // ذخیره در دیتابیس
                var createdProduct = await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // تایید تراکنش
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                // تبدیل به DTO و بازگشت
                return _mapper.Map<ProductDto>(createdProduct);
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