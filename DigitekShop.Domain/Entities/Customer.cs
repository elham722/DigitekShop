using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using DigitekShop.Domain.Events;

namespace DigitekShop.Domain.Entities
{
    public class Customer : BaseAggregateRoot
    {
        // Business Information
        public string CustomerNumber { get; private set; }
        public CustomerStatus Status { get; private set; }
        public CustomerType Type { get; private set; }

        // Personal Information
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? MiddleName { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string? Gender { get; private set; }
        public string? NationalCode { get; private set; }
        public string? PassportNumber { get; private set; }

        // Contact Information
        public Email Email { get; private set; }
        public PhoneNumber Phone { get; private set; }
        public Address Address { get; private set; }

        // Profile Information
        public string? ProfileImageUrl { get; private set; }
        public string? Bio { get; private set; }
        public string? Website { get; private set; }
        public string? Company { get; private set; }
        public string? JobTitle { get; private set; }

        // Business Data
        public Money TotalSpent { get; private set; }
        public int TotalOrders { get; private set; }
        public DateTime? LastPurchaseDate { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        // Verification
        public bool IsEmailVerified { get; private set; }
        public bool IsPhoneVerified { get; private set; }

        // Integration with Identity
        public string? UserId { get; private set; } // Optional link to Identity User

        // Preferences
        public string? PreferredLanguage { get; private set; }
        public string? TimeZone { get; private set; }
        public bool EmailNotifications { get; private set; }
        public bool SmsNotifications { get; private set; }
        public bool PushNotifications { get; private set; }

        // Notes and Audit
        public string? Notes { get; private set; }
        public string? CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }

        // Navigation Properties
        public ICollection<Order> Orders { get; private set; }
        public ICollection<Review> Reviews { get; private set; }
        public Wishlist Wishlist { get; private set; }

        // Constructor
        private Customer()
        {
            Orders = new List<Order>();
            Reviews = new List<Review>();
            TotalSpent = new Money(0, "IRR");
            Status = CustomerStatus.PendingVerification;
            Type = CustomerType.Regular;
            IsEmailVerified = false;
            IsPhoneVerified = false;
            EmailNotifications = true;
            SmsNotifications = false;
            PushNotifications = true;
            PreferredLanguage = "fa-IR";
            TimeZone = "Asia/Tehran";
        }

        public static Customer Create(string firstName, string lastName, Email email, PhoneNumber phone, 
            string? middleName = null, DateTime? dateOfBirth = null, string? nationalCode = null, 
            string? gender = null, CustomerType type = CustomerType.Regular)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty");

            var customer = new Customer
            {
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                MiddleName = middleName?.Trim(),
                DateOfBirth = dateOfBirth,
                NationalCode = nationalCode?.Trim(),
                Gender = gender?.Trim(),
                Email = email,
                Phone = phone,
                Type = type,
                CustomerNumber = GenerateCustomerNumber(),
                Status = CustomerStatus.PendingVerification
            };

            customer.AddDomainEvent(new CustomerCreatedEvent(customer.Id, customer.CustomerNumber, customer.GetFullName(), customer.Email.Value));
            return customer;
        }

        // Business Methods
        public void UpdatePersonalInfo(string firstName, string lastName, string? middleName = null, 
            DateTime? dateOfBirth = null, string? gender = null)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName.Trim();

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName.Trim();

            MiddleName = middleName?.Trim();
            DateOfBirth = dateOfBirth;
            Gender = gender?.Trim();
            SetUpdated();
        }

        public void UpdateContactInfo(Email email, PhoneNumber phone)
        {
            Email = email;
            Phone = phone;
            IsEmailVerified = false; // نیاز به تایید مجدد
            IsPhoneVerified = false; // نیاز به تایید مجدد
            SetUpdated();
        }

        public void UpdateAddress(Address address)
        {
            Address = address;
            SetUpdated();
        }

        public void UpdateProfileInfo(string? profileImageUrl = null, string? bio = null, 
            string? website = null, string? company = null, string? jobTitle = null)
        {
            ProfileImageUrl = profileImageUrl;
            Bio = bio;
            Website = website;
            Company = company;
            JobTitle = jobTitle;
            SetUpdated();
        }

        public void UpdatePreferences(string? preferredLanguage = null, string? timeZone = null,
            bool? emailNotifications = null, bool? smsNotifications = null, bool? pushNotifications = null)
        {
            if (preferredLanguage != null) PreferredLanguage = preferredLanguage;
            if (timeZone != null) TimeZone = timeZone;
            if (emailNotifications.HasValue) EmailNotifications = emailNotifications.Value;
            if (smsNotifications.HasValue) SmsNotifications = smsNotifications.Value;
            if (pushNotifications.HasValue) PushNotifications = pushNotifications.Value;
            SetUpdated();
        }

        public void VerifyEmail()
        {
            IsEmailVerified = true;
            if (IsPhoneVerified && Status == CustomerStatus.PendingVerification)
            {
                Activate();
            }
            SetUpdated();
        }

        public void VerifyPhone()
        {
            IsPhoneVerified = true;
            if (IsEmailVerified && Status == CustomerStatus.PendingVerification)
            {
                Activate();
            }
            SetUpdated();
        }

        public void Activate()
        {
            Status = CustomerStatus.Active;
            SetUpdated();
        }

        public void Deactivate()
        {
            Status = CustomerStatus.Inactive;
            SetUpdated();
        }

        public void Block()
        {
            Status = CustomerStatus.Blocked;
            SetUpdated();
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
            SetUpdated();
        }

        public void UpdateLastPurchase()
        {
            LastPurchaseDate = DateTime.UtcNow;
            SetUpdated();
        }

        public void UpdateTotalSpent(Money amount)
        {
            TotalSpent = amount;
            SetUpdated();
        }

        public void IncrementOrderCount()
        {
            TotalOrders++;
            SetUpdated();
        }

        public void LinkToUser(string userId)
        {
            UserId = userId;
            SetUpdated();
        }

        public void UnlinkFromUser()
        {
            UserId = null;
            SetUpdated();
        }

        public void UpdateNotes(string? notes)
        {
            Notes = notes?.Trim();
            SetUpdated();
        }

        // Business Queries
        public string GetFullName() => $"{FirstName} {LastName}".Trim();
        
        public string GetDisplayName() => GetFullName();
        
        public int GetAge() => DateOfBirth.HasValue ? DateTime.UtcNow.Year - DateOfBirth.Value.Year : 0;
        
        public bool IsActive() => Status == CustomerStatus.Active;
        
        public bool IsBlocked() => Status == CustomerStatus.Blocked;
        
        public bool IsVerified() => IsEmailVerified && IsPhoneVerified;
        
        public bool IsLinkedToUser() => !string.IsNullOrEmpty(UserId);
        
        public int GetOrderCount() => Orders.Count;
        
        public int GetReviewCount() => Reviews.Count;
        
        public decimal GetTotalSpentAmount() => TotalSpent.Amount;
        
        public bool HasActiveOrders()
        {
            return Orders.Any(o => o.Status == OrderStatus.Pending || 
                                  o.Status == OrderStatus.Confirmed || 
                                  o.Status == OrderStatus.Processing || 
                                  o.Status == OrderStatus.Shipped);
        }

        public bool IsNewCustomer() => DateTime.UtcNow.Subtract(CreatedAt).Days <= 30;

        public bool IsVipCustomer() => TotalSpent.Amount > 10000000; // 10M IRR

        // Private Methods
        private static string GenerateCustomerNumber()
        {
            return $"CUST{DateTime.UtcNow:yyyyMMdd}{new Random().Next(1000, 9999)}";
        }

        private void SetUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 