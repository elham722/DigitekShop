using Microsoft.AspNetCore.Mvc;
using DigitekShop.Application.Features.Customers.Queries.GetCustomers;
using DigitekShop.Application.Features.Customers.Commands.CreateCustomer;
using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.DTOs.Customer;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Responses;
using DigitekShop.Api.Attributes;
using DigitekShop.Application.Exceptions;
using DigitekShop.Domain.Exceptions;

namespace DigitekShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CorsPolicy]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IMediator mediator, ILogger<CustomersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// دریافت لیست مشتریان
        /// </summary>
        /// <param name="pageNumber">شماره صفحه</param>
        /// <param name="pageSize">اندازه صفحه</param>
        /// <param name="searchTerm">عبارت جستجو</param>
        /// <param name="status">وضعیت مشتری</param>
        /// <param name="sortBy">مرتب‌سازی بر اساس</param>
        /// <param name="isAscending">مرتب‌سازی صعودی</param>
        /// <returns>لیست مشتریان</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<CustomerDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetCustomers(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? status = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isAscending = true)
        {
            try
            {
                _logger.LogInformation("Getting customers with page {PageNumber}, size {PageSize}", pageNumber, pageSize);
                
                var query = new GetCustomersQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                    Status = status,
                    SortBy = sortBy,
                    IsAscending = isAscending
                };
                
                var result = await _mediator.Send(query);
                
                _logger.LogInformation("Retrieved {Count} customers", result.Items?.Count() ?? 0);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customers");
                return StatusCode(500, new ErrorResponse("خطا در دریافت لیست مشتریان"));
            }
        }

        /// <summary>
        /// دریافت مشتری با شناسه
        /// </summary>
        /// <param name="id">شناسه مشتری</param>
        /// <returns>اطلاعات مشتری</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                _logger.LogInformation("Getting customer with ID: {CustomerId}", id);
                
                var query = new GetCustomersQuery
                {
                    PageNumber = 1,
                    PageSize = 1,
                    SearchTerm = id.ToString()
                };
                
                var result = await _mediator.Send(query);
                
                if (result.Items == null || !result.Items.Any())
                {
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", id);
                    return NotFound(new ErrorResponse("مشتری مورد نظر یافت نشد"));
                }
                
                _logger.LogInformation("Retrieved customer with ID: {CustomerId}", id);
                
                return Ok(result.Items.First());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer with ID: {CustomerId}", id);
                return StatusCode(500, new ErrorResponse("خطا در دریافت اطلاعات مشتری"));
            }
        }

        /// <summary>
        /// ایجاد مشتری جدید
        /// </summary>
        /// <param name="createCustomerDto">اطلاعات مشتری جدید</param>
        /// <returns>مشتری ایجاد شده</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<CustomerDto>), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
        {
            try
            {
                _logger.LogInformation("Creating new customer with email: {Email}", createCustomerDto.Email);
                
                var command = new CreateCustomerCommand
                {
                    FirstName = createCustomerDto.FirstName,
                    LastName = createCustomerDto.LastName,
                    Email = createCustomerDto.Email,
                    Phone = createCustomerDto.Phone,
                    DateOfBirth = createCustomerDto.DateOfBirth,
                    NationalCode = createCustomerDto.NationalCode,
                    ProfileImageUrl = createCustomerDto.ProfileImageUrl,
                    Notes = createCustomerDto.Notes,
                    Street = createCustomerDto.Street,
                    City = createCustomerDto.City,
                    State = createCustomerDto.State,
                    PostalCode = createCustomerDto.PostalCode,
                    Country = createCustomerDto.Country
                };
                
                var result = await _mediator.Send(command);
                
                _logger.LogInformation("Customer created successfully with ID: {CustomerId}", result.Id);
                
                return CreatedAtAction(nameof(GetCustomerById), new { id = result.Id }, 
                    new SuccessResponse<CustomerDto>(result, "مشتری با موفقیت ایجاد شد"));
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error while creating customer: {Message}", ex.Message);
                return BadRequest(new ErrorResponse(ex.Errors, "خطا در اعتبارسنجی اطلاعات مشتری"));
            }
            catch (DuplicateEntityException ex)
            {
                _logger.LogWarning("Duplicate entity error while creating customer: {Message}", ex.Message);
                return BadRequest(new ErrorResponse(ex.Message, "خطا در ایجاد مشتری"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating customer");
                return StatusCode(500, new ErrorResponse("خطا در ایجاد مشتری"));
            }
        }
    }
}