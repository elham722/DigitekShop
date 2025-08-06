using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DigitekShop.Application.DTOs.Common;
using DigitekShop.Application.Exceptions;
using DigitekShop.Domain.Exceptions;
using DigitekShop.Application.Responses;
using FluentValidation;

namespace DigitekShop.Application.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            ErrorResponse errorResponse;

            switch (exception)
            {
                case ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateValidationError(validationEx.Errors);
                    break;

                // Identity Exceptions (must come before BadRequestException)
                case UserNotFoundException userNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateNotFound("User", userNotFoundEx.UserId ?? userNotFoundEx.Email);
                    break;

                case RoleNotFoundException roleNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateNotFound("Role", roleNotFoundEx.RoleId ?? roleNotFoundEx.RoleName);
                    break;

                case PermissionNotFoundException permissionNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateNotFound("Permission", permissionNotFoundEx.PermissionId ?? permissionNotFoundEx.PermissionName);
                    break;

                case NotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateError(notFoundEx.Message, "NOT_FOUND");
                    break;
                case InvalidPasswordException invalidPasswordEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateError(invalidPasswordEx.Message, "INVALID_PASSWORD");
                    break;

                case InvalidTokenException invalidTokenEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateError(invalidTokenEx.Message, "INVALID_TOKEN");
                    break;

                case TwoFactorAuthenticationException twoFactorEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateError(twoFactorEx.Message, "TWO_FACTOR_ERROR");
                    break;

                case TermsNotAcceptedException termsNotAcceptedEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateError(termsNotAcceptedEx.Message, "TERMS_NOT_ACCEPTED");
                    break;

                case PasswordExpiredException passwordExpiredEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateError(passwordExpiredEx.Message, "PASSWORD_EXPIRED");
                    break;

                case DuplicateUserException duplicateUserEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse = ResponseFactory.CreateDuplicate("User", "Email", duplicateUserEx.Email ?? duplicateUserEx.UserName);
                    break;

                case DuplicateRoleException duplicateRoleEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse = ResponseFactory.CreateDuplicate("Role", "Name", duplicateRoleEx.RoleName);
                    break;

                case DuplicatePermissionException duplicatePermissionEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse = ResponseFactory.CreateDuplicate("Permission", "Name", duplicatePermissionEx.PermissionName);
                    break;

                case BadRequestException badRequestEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateError(badRequestEx.Message, "BAD_REQUEST");
                    break;

                // Domain Exceptions
                case ProductNotFoundException productNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateNotFound("Product", productNotFoundEx.ProductId);
                    break;

                case CustomerNotFoundException customerNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateNotFound("Customer", customerNotFoundEx.CustomerId);
                    break;

                case CategoryNotFoundException categoryNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateNotFound("Category", categoryNotFoundEx.CategoryId);
                    break;

                case BrandNotFoundException brandNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateNotFound("Brand", brandNotFoundEx.BrandId);
                    break;

                case OrderNotFoundException orderNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = ResponseFactory.CreateNotFound("Order", orderNotFoundEx.OrderId);
                    break;

                case DuplicateEntityException duplicateEntityEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    errorResponse = ResponseFactory.CreateDuplicate(
                        duplicateEntityEx.EntityType, 
                        duplicateEntityEx.PropertyName, 
                        duplicateEntityEx.PropertyValue);
                    break;



                case SessionExpiredException sessionExpiredEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse = ResponseFactory.CreateError(sessionExpiredEx.Message, "SESSION_EXPIRED");
                    break;

                case InvalidCredentialsException invalidCredentialsEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse = ResponseFactory.CreateError(invalidCredentialsEx.Message, "INVALID_CREDENTIALS");
                    break;

                case EmailNotConfirmedException emailNotConfirmedEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateError(emailNotConfirmedEx.Message, "EMAIL_NOT_CONFIRMED");
                    break;

                case AccountLockedException accountLockedEx:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse = ResponseFactory.CreateError(accountLockedEx.Message, "ACCOUNT_LOCKED");
                    break;

                // Domain Exceptions (must come before DomainException)
                case InvalidProductDataException invalidProductDataEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateError(invalidProductDataEx.Message, "INVALID_DATA");
                    break;

                case InsufficientStockException insufficientStockEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateBusinessRuleViolation(insufficientStockEx.Message);
                    break;

                case InvalidOrderStatusException invalidOrderStatusEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateBusinessRuleViolation(invalidOrderStatusEx.Message);
                    break;

                case CustomerNotActiveException customerNotActiveEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateBusinessRuleViolation(customerNotActiveEx.Message);
                    break;

                case DomainException domainEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = ResponseFactory.CreateBusinessRuleViolation(domainEx.Message);
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = ResponseFactory.CreateError("An unexpected error occurred.", "INTERNAL_ERROR");
                    
                    // Log the exception for debugging
                    _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
                    break;
            }

            // Wrap the error response with context information
            errorResponse = ResponseWrapper.Wrap(errorResponse, context);

            var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await response.WriteAsync(result);
        }
    }
} 