using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Customer;
using DigitekShop.Application.Interfaces;
using DigitekShop.Domain.Interfaces;
using AutoMapper;
using System.Linq;

namespace DigitekShop.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, PagedResultDto<CustomerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<CustomerDto>> HandleAsync(GetCustomersQuery query, CancellationToken cancellationToken = default)
        {
            // دریافت همه مشتریان فعال
            var allCustomers = await _unitOfWork.Customers.GetActiveAsync();
            
            // اعمال فیلترها
            var filteredCustomers = allCustomers.AsQueryable();
            
            // فیلتر بر اساس جستجو
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                filteredCustomers = filteredCustomers.Where(c => 
                    c.FirstName.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.LastName.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Value.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            
            // فیلتر بر اساس وضعیت
            if (!string.IsNullOrEmpty(query.Status))
            {
                filteredCustomers = filteredCustomers.Where(c => c.Status.ToString() == query.Status);
            }
            
            // محاسبه تعداد کل
            var totalCount = filteredCustomers.Count();
            
            // اعمال مرتب‌سازی
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                filteredCustomers = query.SortBy.ToLower() switch
                {
                    "firstname" => query.IsAscending ? filteredCustomers.OrderBy(c => c.FirstName) : filteredCustomers.OrderByDescending(c => c.FirstName),
                    "lastname" => query.IsAscending ? filteredCustomers.OrderBy(c => c.LastName) : filteredCustomers.OrderByDescending(c => c.LastName),
                    "email" => query.IsAscending ? filteredCustomers.OrderBy(c => c.Email.Value) : filteredCustomers.OrderByDescending(c => c.Email.Value),
                    "createdat" => query.IsAscending ? filteredCustomers.OrderBy(c => c.CreatedAt) : filteredCustomers.OrderByDescending(c => c.CreatedAt),
                    _ => filteredCustomers.OrderBy(c => c.FirstName)
                };
            }
            else
            {
                filteredCustomers = filteredCustomers.OrderBy(c => c.FirstName);
            }
            
            // اعمال pagination
            var skip = (query.PageNumber - 1) * query.PageSize;
            var pagedCustomers = filteredCustomers.Skip(skip).Take(query.PageSize).ToList();
            
            // تبدیل به DTO
            var customerDtos = pagedCustomers.Select(c => _mapper.Map<CustomerDto>(c)).ToList();
            
            // ایجاد نتیجه paginated
            return new PagedResultDto<CustomerDto>
            {
                Items = customerDtos,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
} 