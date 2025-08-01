using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Order;
using DigitekShop.Application.Interfaces;
using DigitekShop.Domain.Interfaces;
using AutoMapper;
using System.Linq;

namespace DigitekShop.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PagedResultDto<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<OrderDto>> HandleAsync(GetOrdersQuery query, CancellationToken cancellationToken = default)
        {
            // دریافت همه سفارش‌ها
            var allOrders = await _unitOfWork.Orders.GetAllAsync();
            
            // اعمال فیلترها
            var filteredOrders = allOrders.AsQueryable();
            
            // فیلتر بر اساس مشتری
            if (query.CustomerId.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.CustomerId == query.CustomerId.Value);
            }
            
            // فیلتر بر اساس وضعیت
            if (!string.IsNullOrEmpty(query.Status))
            {
                filteredOrders = filteredOrders.Where(o => o.Status.ToString() == query.Status);
            }
            
            // فیلتر بر اساس تاریخ
            if (query.FromDate.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.CreatedAt >= query.FromDate.Value);
            }
            
            if (query.ToDate.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.CreatedAt <= query.ToDate.Value);
            }
            
            // محاسبه تعداد کل
            var totalCount = filteredOrders.Count();
            
            // اعمال مرتب‌سازی
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                filteredOrders = query.SortBy.ToLower() switch
                {
                    "createdat" => query.IsAscending ? filteredOrders.OrderBy(o => o.CreatedAt) : filteredOrders.OrderByDescending(o => o.CreatedAt),
                    "totalamount" => query.IsAscending ? filteredOrders.OrderBy(o => o.TotalAmount.Amount) : filteredOrders.OrderByDescending(o => o.TotalAmount.Amount),
                    "status" => query.IsAscending ? filteredOrders.OrderBy(o => o.Status) : filteredOrders.OrderByDescending(o => o.Status),
                    "customername" => query.IsAscending ? filteredOrders.OrderBy(o => o.Customer.FirstName) : filteredOrders.OrderByDescending(o => o.Customer.FirstName),
                    _ => filteredOrders.OrderByDescending(o => o.CreatedAt)
                };
            }
            else
            {
                filteredOrders = filteredOrders.OrderByDescending(o => o.CreatedAt);
            }
            
            // اعمال pagination
            var skip = (query.PageNumber - 1) * query.PageSize;
            var pagedOrders = filteredOrders.Skip(skip).Take(query.PageSize).ToList();
            
            // تبدیل به DTO
            var orderDtos = pagedOrders.Select(o => _mapper.Map<OrderDto>(o)).ToList();
            
            // ایجاد نتیجه paginated
            return new PagedResultDto<OrderDto>
            {
                Items = orderDtos,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
} 