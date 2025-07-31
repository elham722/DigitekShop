using System;
using DigitekShop.Domain.Entities.Common;

namespace DigitekShop.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public int CustomerId { get; private set; }
        public int ProductId { get; private set; }
        public DateTime AddedAt { get; private set; }
        public string Notes { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation Properties
        public Customer Customer { get; private set; }
        public Product Product { get; private set; }

        // Constructor
        private Wishlist() { }

        public Wishlist(int customerId, int productId, string notes = "")
        {
            CustomerId = customerId;
            ProductId = productId;
            AddedAt = DateTime.UtcNow;
            Notes = notes?.Trim() ?? "";
            IsActive = true;
            SetUpdated();
        }

        // Business Methods
        public void UpdateNotes(string notes)
        {
            Notes = notes?.Trim() ?? "";
            SetUpdated();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdated();
        }

        public void Activate()
        {
            IsActive = true;
            SetUpdated();
        }

        // Business Queries
        public bool IsActiveItem() => IsActive && !IsDeleted;
        
        public TimeSpan GetAge()
        {
            return DateTime.UtcNow - AddedAt;
        }
        
        public bool IsRecentlyAdded(int days = 7)
        {
            return GetAge().TotalDays <= days;
        }
    }
} 