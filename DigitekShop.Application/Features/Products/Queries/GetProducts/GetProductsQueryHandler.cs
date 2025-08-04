using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Product;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.Enums;
using AutoMapper;
using System.Linq;
using MediatR;

namespace DigitekShop.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : MediatR.IRequestHandler<GetProductsQuery, PagedResultDto<ProductListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<ProductListDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            // دریافت همه محصولات فعال
            var allProducts = await _unitOfWork.Products.GetByStatusAsync(ProductStatus.Active);
            
            // اعمال فیلترها
            var filteredProducts = allProducts.AsQueryable();
            
            // فیلتر بر اساس جستجو
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                filteredProducts = filteredProducts.Where(p => 
                    p.Name.Value.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            
            // فیلتر بر اساس دسته‌بندی
            if (query.CategoryId.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryId == query.CategoryId.Value);
            }
            
            // فیلتر بر اساس برند
            if (query.BrandId.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.BrandId == query.BrandId.Value);
            }
            
            // فیلتر بر اساس قیمت
            if (query.MinPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price.Amount >= query.MinPrice.Value);
            }
            
            if (query.MaxPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price.Amount <= query.MaxPrice.Value);
            }
            
            // فیلتر بر اساس موجودی
            if (query.InStockOnly == true)
            {
                filteredProducts = filteredProducts.Where(p => p.StockQuantity > 0);
            }
            
            // محاسبه تعداد کل
            var totalCount = filteredProducts.Count();
            
            // اعمال مرتب‌سازی
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                filteredProducts = query.SortBy.ToLower() switch
                {
                    "name" => query.IsAscending ? filteredProducts.OrderBy(p => p.Name.Value) : filteredProducts.OrderByDescending(p => p.Name.Value),
                    "price" => query.IsAscending ? filteredProducts.OrderBy(p => p.Price.Amount) : filteredProducts.OrderByDescending(p => p.Price.Amount),
                    "stock" => query.IsAscending ? filteredProducts.OrderBy(p => p.StockQuantity) : filteredProducts.OrderByDescending(p => p.StockQuantity),
                    "createdat" => query.IsAscending ? filteredProducts.OrderBy(p => p.CreatedAt) : filteredProducts.OrderByDescending(p => p.CreatedAt),
                    _ => filteredProducts.OrderBy(p => p.Name.Value)
                };
            }
            else
            {
                filteredProducts = filteredProducts.OrderBy(p => p.Name.Value);
            }
            
            // اعمال pagination
            var skip = (query.PageNumber - 1) * query.PageSize;
            var pagedProducts = filteredProducts.Skip(skip).Take(query.PageSize).ToList();
            
            // تبدیل به DTO
            var productDtos = pagedProducts.Select(p => _mapper.Map<ProductListDto>(p)).ToList();
            
            // ایجاد نتیجه paginated
            return new PagedResultDto<ProductListDto>
            {
                Items = productDtos,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
                // TotalPages به صورت خودکار محاسبه می‌شود
            };
        }
    }
} 