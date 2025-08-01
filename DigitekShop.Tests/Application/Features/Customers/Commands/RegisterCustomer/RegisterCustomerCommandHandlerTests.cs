using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitekShop.Application.DTOs.Customer;
using DigitekShop.Application.Features.Customers.Commands.RegisterCustomer;
using DigitekShop.Application.Interfaces.Infrastructure;
using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Interfaces;
using DigitekShop.Domain.ValueObjects;
using Moq;
using Xunit;

namespace DigitekShop.Tests.Application.Features.Customers.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IEmailTemplateService> _mockEmailTemplateService;
        private readonly RegisterCustomerCommandHandler _handler;

        public RegisterCustomerCommandHandlerTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockEmailSender = new Mock<IEmailSender>();
            _mockEmailTemplateService = new Mock<IEmailTemplateService>();

            _handler = new RegisterCustomerCommandHandler(
                _mockCustomerRepository.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockEmailSender.Object,
                _mockEmailTemplateService.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldSendWelcomeEmail()
        {
            // Arrange
            var command = new RegisterCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                PhoneNumber = "+1234567890",
                Street = "123 Main St",
                City = "New York",
                State = "NY",
                ZipCode = "10001",
                Country = "USA"
            };

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = new Email(command.Email),
                PhoneNumber = new PhoneNumber(command.PhoneNumber),
                Address = new Address(command.Street, command.City, command.State, command.ZipCode, command.Country),
                Status = Domain.Enums.CustomerStatus.Active,
                CreatedDate = DateTime.UtcNow
            };

            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email.Value,
                PhoneNumber = customer.PhoneNumber.Value,
                Street = customer.Address.Street,
                City = customer.Address.City,
                State = customer.Address.State,
                ZipCode = customer.Address.ZipCode,
                Country = customer.Address.Country,
                Status = customer.Status.ToString(),
                CreatedDate = customer.CreatedDate
            };

            _mockCustomerRepository.Setup(repo => repo.GetCustomerByEmailAsync(It.IsAny<Email>()))
                .ReturnsAsync((Customer)null);

            _mockCustomerRepository.Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mockMapper.Setup(mapper => mapper.Map<CustomerDto>(It.IsAny<Customer>()))
                .Returns(customerDto);

            _mockEmailTemplateService.Setup(service => service.GetProcessedTemplateAsync("WelcomeEmail", It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync("<html><body>Welcome Email Content</body></html>");

            _mockEmailSender.Setup(sender => sender.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Id, result.Id);
            
            // Verify that the email template service was called with the correct parameters
            _mockEmailTemplateService.Verify(service => service.GetProcessedTemplateAsync(
                "WelcomeEmail", 
                It.Is<Dictionary<string, string>>(dict => 
                    dict.ContainsKey("FirstName") && dict["FirstName"] == command.FirstName &&
                    dict.ContainsKey("Email") && dict["Email"] == command.Email
                )), 
                Times.Once);

            // Verify that the email sender was called with the correct parameters
            _mockEmailSender.Verify(sender => sender.SendEmailAsync(
                command.Email,
                "Welcome to DigitekShop!",
                "<html><body>Welcome Email Content</body></html>",
                true), 
                Times.Once);
        }
    }
}