using DigitekShop.Application.DTOs.Customer;
using DigitekShop.Application.Interfaces;
using DigitekShop.Application.Profiles;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Exceptions;
using AutoMapper;

namespace DigitekShop.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDto> HandleAsync(CreateCustomerCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // شروع تراکنش
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                // بررسی تکراری نبودن ایمیل
                var existingCustomerByEmail = await _unitOfWork.Customers.GetByEmailAsync(new Email(command.Email));
                if (existingCustomerByEmail != null)
                    throw new DuplicateEntityException("Customer", "email", command.Email);

                // بررسی تکراری نبودن شماره تلفن
                var existingCustomerByPhone = await _unitOfWork.Customers.GetByPhoneAsync(new PhoneNumber(command.Phone));
                if (existingCustomerByPhone != null)
                    throw new DuplicateEntityException("Customer", "phone", command.Phone);

                // ایجاد Value Objects
                var email = new Email(command.Email);
                var phone = new PhoneNumber(command.Phone);

                // ایجاد آدرس (اگر ارائه شده)
                Address? address = null;
                if (!string.IsNullOrEmpty(command.Address) || !string.IsNullOrEmpty(command.City))
                {
                    address = new Address(
                        command.State ?? "",      // Province
                        command.City ?? "",       // City
                        "",                       // District
                        command.Address ?? "",    // Street
                        command.PostalCode ?? "", // PostalCode
                        ""                        // Details
                    );
                }

                // ایجاد مشتری جدید
                var customer = Customer.Create(
                    command.FirstName,
                    command.LastName,
                    email,
                    phone,
                    command.MiddleName,
                    command.DateOfBirth,
                    command.NationalCode,
                    command.Gender,
                    command.Type
                );

                // تنظیم آدرس
                if (address != null)
                {
                    customer.UpdateAddress(address);
                }

                // تنظیم اطلاعات پروفایل
                if (!string.IsNullOrEmpty(command.Bio) || !string.IsNullOrEmpty(command.Website) || 
                    !string.IsNullOrEmpty(command.Company) || !string.IsNullOrEmpty(command.JobTitle))
                {
                    customer.UpdateProfileInfo(
                        null, // profileImageUrl
                        command.Bio,
                        command.Website,
                        command.Company,
                        command.JobTitle
                    );
                }

                // لینک کردن با User (اگر ارائه شده)
                if (!string.IsNullOrEmpty(command.UserId))
                {
                    customer.LinkToUser(command.UserId);
                }

                // تنظیم یادداشت‌ها
                if (!string.IsNullOrEmpty(command.Notes))
                {
                    customer.UpdateNotes(command.Notes);
                }

                // ذخیره در دیتابیس
                var createdCustomer = await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // تایید تراکنش
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                // تبدیل به DTO و بازگشت
                return _mapper.Map<CustomerDto>(createdCustomer);
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