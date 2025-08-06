using System;
using System.Linq;
using DigitekShop.Domain.Exceptions;

namespace DigitekShop.Application.Exceptions
{
    public static class ExceptionFactory
    {
        public static NotFoundException EntityNotFound(string entityName, object key)
        {
            return new NotFoundException(entityName, key);
        }

        public static NotFoundException UserNotFound(string userId)
        {
            return new NotFoundException("User", userId);
        }

        public static NotFoundException UserNotFoundByEmail(string email)
        {
            return new NotFoundException($"User with email '{email}' was not found");
        }

        public static DuplicateEntityException DuplicateEmail(string email)
        {
            return new DuplicateEntityException("User", "Email", email);
        }

        public static DuplicateEntityException DuplicateUserName(string userName)
        {
            return new DuplicateEntityException("User", "UserName", userName);
        }

        public static DuplicateEntityException DuplicatePhoneNumber(string phoneNumber)
        {
            return new DuplicateEntityException("User", "PhoneNumber", phoneNumber);
        }

        public static InvalidCredentialsException InvalidCredentials(string email, int remainingAttempts = 0)
        {
            return new InvalidCredentialsException(email, remainingAttempts);
        }

        public static AccountLockedException AccountLocked(string userId, DateTime? lockoutEnd = null, string reason = null)
        {
            return new AccountLockedException(userId, lockoutEnd, reason);
        }

        public static EmailNotConfirmedException EmailNotConfirmed(string email, bool resendTokenAvailable = true)
        {
            return new EmailNotConfirmedException(email, resendTokenAvailable);
        }

        public static UnauthorizedException Unauthorized(string resource, string action)
        {
            return new UnauthorizedException(resource, action);
        }

        public static ForbiddenException Forbidden(string userId, string requiredPermission, string resource = null)
        {
            return new ForbiddenException(userId, requiredPermission, resource);
        }

        public static InvalidOperationException InvalidOperation(string operation, string entityName, string reason)
        {
            return new InvalidOperationException(operation, entityName, reason);
        }

        public static BadRequestException BadRequest(string message)
        {
            return new BadRequestException(message);
        }

        public static ApplicationValidationException ValidationError(string message)
        {
            return new ApplicationValidationException(message);
        }

        public static ApplicationValidationException ValidationError(IEnumerable<FluentValidation.Results.ValidationFailure> failures)
        {
            return new ApplicationValidationException(failures);
        }
    }
} 