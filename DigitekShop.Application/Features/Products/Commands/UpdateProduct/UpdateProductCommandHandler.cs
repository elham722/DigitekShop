using DigitekShop.Application.DTOs.Product;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Profiles;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Exceptions;
using AutoMapper;

namespace DigitekShop.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> HandleAsync(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // شروع تراکنش
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                // دریافت محصول موجود
                var product = await _unitOfWork.Products.GetByIdAsync(command.Id);
                if (product == null)
                    throw new ProductNotFoundException(command.Id);

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

                // به‌روزرسانی اطلاعات محصول
                if (!string.IsNullOrEmpty(command.Name))
                {
                    var productName = new ProductName(command.Name);
                    product.UpdateName(productName);
                }

                if (!string.IsNullOrEmpty(command.Description))
                {
                    product.UpdateDescription(command.Description);
                }

                var price = new Money(command.Price, "IRR");
                product.UpdatePrice(price);
                product.UpdateStock(command.StockQuantity);

                // به‌روزرسانی تصویر
                if (!string.IsNullOrEmpty(command.ImageUrl))
                {
                    product.UpdateImageUrl(command.ImageUrl);
                }

                // به‌روزرسانی محصول در UnitOfWork
                await _unitOfWork.Products.UpdateAsync(product);

                // ذخیره تغییرات
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // تایید تراکنش
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                // تبدیل به DTO و بازگشت
                return _mapper.Map<ProductDto>(product);
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