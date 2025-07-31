using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;

namespace DigitekShop.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public ProductName Name { get; private set; }
        public string Description { get; private set; }
        public string LogoUrl { get; private set; }
        public string Website { get; private set; }
        public string Country { get; private set; }
        public int FoundedYear { get; private set; }
        public bool IsActive { get; private set; }
        public int DisplayOrder { get; private set; }
        public string Slug { get; private set; }

        // Navigation Properties
        public ICollection<Product> Products { get; private set; }

        // Constructor
        private Brand()
        {
            Products = new List<Product>();
        }

        public Brand(ProductName name, string description, string country = "", int foundedYear = 0)
        {
            Name = name;
            Description = description ?? "";
            Country = country?.Trim() ?? "";
            FoundedYear = foundedYear;
            IsActive = true;
            DisplayOrder = 0;
            Slug = GenerateSlug(name.Value);
            
            Products = new List<Product>();
            SetUpdated();
        }

        // Business Methods
        public void UpdateInfo(string description, string website, string country, int foundedYear)
        {
            Description = description ?? "";
            Website = website?.Trim() ?? "";
            Country = country?.Trim() ?? "";
            FoundedYear = foundedYear;
            SetUpdated();
        }

        public void UpdateLogo(string logoUrl)
        {
            LogoUrl = logoUrl;
            SetUpdated();
        }

        public void UpdateDisplayOrder(int order)
        {
            DisplayOrder = order;
            SetUpdated();
        }

        public void Activate()
        {
            IsActive = true;
            SetUpdated();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdated();
        }

        public void UpdateName(ProductName newName)
        {
            Name = newName;
            Slug = GenerateSlug(newName.Value);
            SetUpdated();
        }

        // Business Queries
        public string GetDisplayName() => Name.Value;
        
        public int GetProductCount() => Products.Count;
        
        public bool HasProducts() => Products.Any();
        
        public bool IsEstablished() => FoundedYear > 0;
        
        public int GetAge()
        {
            if (FoundedYear <= 0) return 0;
            return DateTime.Now.Year - FoundedYear;
        }

        private string GenerateSlug(string name)
        {
            return name.ToLower()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("(", "")
                .Replace(")", "");
        }
    }
} 