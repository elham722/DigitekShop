using System;
using DigitekShop.Domain.Entities.Common;

namespace DigitekShop.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int CustomerId { get; private set; }
        public int ProductId { get; private set; }
        public int Rating { get; private set; } // 1-5
        public string Title { get; private set; }
        public string Comment { get; private set; }
        public bool IsVerified { get; private set; }
        public bool IsHelpful { get; private set; }
        public int HelpfulCount { get; private set; }
        public bool IsApproved { get; private set; }
        public string AdminResponse { get; private set; }

        // Navigation Properties
        public Customer Customer { get; private set; }
        public Product Product { get; private set; }

        // Constructor
        private Review() { }

        public Review(int customerId, int productId, int rating, string title, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty");

            CustomerId = customerId;
            ProductId = productId;
            Rating = rating;
            Title = title.Trim();
            Comment = comment?.Trim() ?? "";
            IsVerified = false;
            IsHelpful = false;
            HelpfulCount = 0;
            IsApproved = false;
            SetUpdated();
        }

        // Business Methods
        public void UpdateReview(int rating, string title, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty");

            Rating = rating;
            Title = title.Trim();
            Comment = comment?.Trim() ?? "";
            SetUpdated();
        }

        public void MarkAsVerified()
        {
            IsVerified = true;
            SetUpdated();
        }

        public void MarkAsHelpful()
        {
            IsHelpful = true;
            HelpfulCount++;
            SetUpdated();
        }

        public void Approve()
        {
            IsApproved = true;
            SetUpdated();
        }

        public void Reject()
        {
            IsApproved = false;
            SetUpdated();
        }

        public void AddAdminResponse(string response)
        {
            AdminResponse = response?.Trim();
            SetUpdated();
        }

        // Business Queries
        public bool IsPositive() => Rating >= 4;
        
        public bool IsNegative() => Rating <= 2;
        
        public bool IsNeutral() => Rating == 3;
        
        public string GetRatingText()
        {
            return Rating switch
            {
                1 => "خیلی بد",
                2 => "بد",
                3 => "متوسط",
                4 => "خوب",
                5 => "عالی",
                _ => "نامشخص"
            };
        }
        
        public bool CanBeEdited() => !IsVerified;
        
        public bool IsPublished() => IsApproved && IsVerified;
    }
} 