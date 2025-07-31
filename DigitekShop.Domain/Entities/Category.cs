using DigitekShop.Domain.Entities.Common;
using DigitekShop.Domain.ValueObjects;
using DigitekShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitekShop.Domain.Entities
{
    public class Category : BaseEntity
    {
        public ProductName Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public CategoryType Type { get; private set; }
        public int DisplayOrder { get; private set; }
        public string Slug { get; private set; }

        // Self-referencing for hierarchical categories
        public int? ParentCategoryId { get; private set; }
        public Category ParentCategory { get; private set; }
        public ICollection<Category> SubCategories { get; private set; }

        // Navigation Properties
        public ICollection<Product> Products { get; private set; }

        // Constructor
        private Category()
        {
            SubCategories = new List<Category>();
            Products = new List<Product>();
        }

        public Category(ProductName name, string description, CategoryType type = CategoryType.Main, 
            int? parentCategoryId = null, int displayOrder = 0)
        {
            Name = name;
            Description = description ?? "";
            Type = type;
            ParentCategoryId = parentCategoryId;
            DisplayOrder = displayOrder;
            Slug = GenerateSlug(name.Value);
            SubCategories = new List<Category>();
            Products = new List<Product>();
            SetUpdated();
        }

        // Business Methods
        public void AddSubCategory(Category subCategory)
        {
            if (subCategory == null)
                throw new ArgumentNullException(nameof(subCategory));

            if (subCategory.Id == Id)
                throw new InvalidOperationException("Category cannot be its own subcategory");

            subCategory.ParentCategoryId = Id;
            SubCategories.Add(subCategory);
            SetUpdated();
        }

        public void RemoveSubCategory(int subCategoryId)
        {
            var subCategory = SubCategories.FirstOrDefault(c => c.Id == subCategoryId);
            if (subCategory != null)
            {
                SubCategories.Remove(subCategory);
                subCategory.ParentCategoryId = null;
                SetUpdated();
            }
        }

        public void UpdateName(ProductName newName)
        {
            Name = newName;
            Slug = GenerateSlug(newName.Value);
            SetUpdated();
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? "";
            SetUpdated();
        }

        public void UpdateImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            SetUpdated();
        }

        public void UpdateDisplayOrder(int newOrder)
        {
            DisplayOrder = newOrder;
            SetUpdated();
        }

        public void ChangeType(CategoryType newType)
        {
            Type = newType;
            SetUpdated();
        }

        public bool IsMainCategory() => Type == CategoryType.Main;
        
        public bool IsSubCategory() => Type == CategoryType.Sub;
        
        public bool HasSubCategories() => SubCategories.Any();
        
        public bool HasProducts() => Products.Any();
        
        public int GetProductCount() => Products.Count;
        
        public int GetSubCategoryCount() => SubCategories.Count;

        public string GetDisplayName() => Name.Value;
        
        public string GetFullPath()
        {
            var path = new List<string> { Name.Value };
            var current = this;
            
            while (current.ParentCategory != null)
            {
                current = current.ParentCategory;
                path.Insert(0, current.Name.Value);
            }
            
            return string.Join(" > ", path);
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
