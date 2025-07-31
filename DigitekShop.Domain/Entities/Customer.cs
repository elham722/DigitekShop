using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;

namespace DigitekShop.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber Phone { get; private set; }
        public Address Address { get; private set; }
        public CustomerStatus Status { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string NationalCode { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public bool IsPhoneVerified { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public string ProfileImageUrl { get; private set; }
        public string Notes { get; private set; }

        // Navigation Properties
        public ICollection<Order> Orders { get; private set; }
        public ICollection<Review> Reviews { get; private set; }
        public ICollection<Wishlist> Wishlists { get; private set; }

        // Constructor
        private Customer()
        {
            Orders = new List<Order>();
            Reviews = new List<Review>();
            Wishlists = new List<Wishlist>();
        }

        public Customer(string firstName, string lastName, Email email, PhoneNumber phone, 
            Address address, string nationalCode = "")
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email;
            Phone = phone;
            Address = address;
            NationalCode = nationalCode?.Trim() ?? "";
            Status = CustomerStatus.PendingVerification;
            IsEmailVerified = false;
            IsPhoneVerified = false;
            
            Orders = new List<Order>();
            Reviews = new List<Review>();
            Wishlists = new List<Wishlist>();
            
            SetUpdated();
        }

        // Business Methods
        public void UpdatePersonalInfo(string firstName, string lastName, DateTime? dateOfBirth)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                FirstName = firstName.Trim();

            if (!string.IsNullOrWhiteSpace(lastName))
                LastName = lastName.Trim();

            DateOfBirth = dateOfBirth;
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

        public void VerifyEmail()
        {
            IsEmailVerified = true;
            if (IsPhoneVerified && Status == CustomerStatus.PendingVerification)
            {
                Status = CustomerStatus.Active;
            }
            SetUpdated();
        }

        public void VerifyPhone()
        {
            IsPhoneVerified = true;
            if (IsEmailVerified && Status == CustomerStatus.PendingVerification)
            {
                Status = CustomerStatus.Active;
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

        public void UpdateProfileImage(string imageUrl)
        {
            ProfileImageUrl = imageUrl;
            SetUpdated();
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes?.Trim() ?? "";
            SetUpdated();
        }

        // Business Queries
        public string GetFullName() => $"{FirstName} {LastName}".Trim();
        
        public string GetDisplayName() => GetFullName();
        
        public bool IsActive() => Status == CustomerStatus.Active;
        
        public bool IsBlocked() => Status == CustomerStatus.Blocked;
        
        public bool IsVerified() => IsEmailVerified && IsPhoneVerified;
        
        public int GetOrderCount() => Orders.Count;
        
        public int GetReviewCount() => Reviews.Count;
        
        public int GetWishlistCount() => Wishlists.Count;
        
        public decimal GetTotalSpent()
        {
            return Orders
                .Where(o => o.Status == OrderStatus.Delivered)
                .Sum(o => o.TotalAmount.Amount);
        }
        
        public bool HasActiveOrders()
        {
            return Orders.Any(o => o.Status == OrderStatus.Pending || 
                                  o.Status == OrderStatus.Confirmed || 
                                  o.Status == OrderStatus.Processing || 
                                  o.Status == OrderStatus.Shipped);
        }
    }
} 