using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Services;
using DigitekShop.Application.Exceptions;
using DigitekShop.Application.Responses;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Exceptions;
using AutoMapper;

namespace DigitekShop.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CommandResponse<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public CreateProductCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<CommandResponse<ProductDto>> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            // اعتبارسنجی Command
            var validationResult = await _validationService.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                // شروع تراکنش
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                // بررسی وجود دسته‌بندی
                var category = await _unitOfWork.Categories.GetByIdAsync(command.CategoryId);
                if (category == null)
                    throw new CategoryNotFoundException(command.CategoryId);

                // بررسی وجود برند (اگر مشخص شده)
                if (command.BrandId.HasValue)
                {
                    var brand = await _unitOfWork.Brands.GetByIdAsync(command.BrandId.Value);
                    if (brand == null)
                        throw new BrandNotFoundException(command.BrandId.Value);
                }

                // بررسی تکراری نبودن SKU
                var existingProduct = await _unitOfWork.Products.GetBySKUAsync(new SKU(command.SKU));
                if (existingProduct != null)
                    throw new DuplicateEntityException("Product", "SKU", command.SKU);

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

                // تبدیل به DTO و ایجاد Response
                var productDto = _mapper.Map<ProductDto>(createdProduct);
                return ResponseFactory.CreateCommandWithDataAndId(
                    productDto, 
                    createdProduct.Id, 
                    "CreateProduct", 
                    "Product created successfully");
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